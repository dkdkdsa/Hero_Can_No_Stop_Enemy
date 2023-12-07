using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using System.Linq;

public delegate void LoginEvent(bool success, FirebaseUser user);
public delegate void CreateAccountEvent(bool success);

[Serializable]
public class FirebaseUserData
{

    public string userName;
    public List<string> deck = new();
    public List<string> ableTower = new();
    public List<string> friends = new();
    public int coin;
    public int loginCount;
    public string loginTime;

}

[Serializable]
public class FirebaseFriendReqData
{

    public List<string> reqs = new();

}

public class FirebaseManager : MonoBehaviour
{

    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference db;

    public FirebaseUserData userData { get; private set; }
    public bool IsContinuousLogIn { get; private set; }
    public bool IsAuthError { get; private set; }

    public static FirebaseManager Instance;

    public void StartAuth()
    {

        Instance = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {

                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;

            }
            else
            {

                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                IsAuthError = true;

            }

        });

    }

    public async void Login(string email, string password, LoginEvent loginEvent)
    {

        try
        {

            var res = await auth.SignInWithEmailAndPasswordAsync(email, password);
            user = res.User;

            loginEvent?.Invoke(true, user);

        }
        catch(Exception ex)
        {

            Debug.LogError(ex.Message);
            loginEvent?.Invoke(false, null);

        }

    }

    public async void CreateAccount(string email, string password, string userName, CreateAccountEvent callback, bool login = true)
    {

        try
        {

            var res = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            user = res.User;

            if (login)
            {

                Login(email, password, (success, user) =>
                {

                    if (success)
                    {

                        CreateUserData(userName);

                    }

                    callback?.Invoke(success);

                });

            }
            else
            {

                callback?.Invoke(true);

            }



        }
        catch (Exception ex)
        {


            Debug.LogError(ex.Message);
            callback?.Invoke(false);

        }

    }

    public void CreateUserData(string userName)
    {

        if (user == null) return;

        userData = new FirebaseUserData 
        { 

            userName = userName, 
            loginTime = DateTime.Now.ToString("f"), 
            loginCount = 1, 
            ableTower = { "Chicken" }, 
            coin = 500 

        };

        IsContinuousLogIn = true;

        DeckManager.Instance.AbleTowerLs = userData.ableTower;

        db.Child("users").Child(user.UserId).Child("UserData").SetValueAsync(JsonUtility.ToJson(userData));
        db.Child("users").Child(user.UserId).Child("FriendReq").SetValueAsync(JsonUtility.ToJson(new FirebaseFriendReqData()));

    }

    public async Task LoadUserdata()
    {

        if (user == null) return;

        var res = await db.Child("users").Child(user.UserId).Child("UserData").GetValueAsync();

        if(res != null && res.Value != null)
        {

            userData = JsonUtility.FromJson<FirebaseUserData>(res.Value.ToString());

            if(userData.loginTime == null)
            {

                userData.loginTime = DateTime.Now.ToString("f");
                userData.loginCount = 1;

            }
            else
            {

                var t = DateTime.Parse(userData.loginTime);

                if ((DateTime.Now - t).TotalMinutes > 1)
                {

                    IsContinuousLogIn = true;
                    userData.loginCount++;

                }
                else if((DateTime.Now - t).TotalDays > 1)
                {

                    IsContinuousLogIn = false;
                    userData.loginCount = 0;

                }

                userData.loginTime = DateTime.Now.ToString("f");

            }

            DeckManager.Instance.DeckLs = userData.deck;
            DeckManager.Instance.AbleTowerLs = userData.ableTower;

        }

        SaveUserData();

    }

    public void SaveUserData()
    {

        if (user == null) return;

        userData.deck = DeckManager.Instance.DeckLs;

        db.Child("users").Child(user.UserId).Child("UserData").SetValueAsync(JsonUtility.ToJson(userData));

    }

    public async Task<List<(string key, FirebaseUserData userData)>> GetAllUser()
    {

        List<(string key, FirebaseUserData userData)> dataLs = new();

        var res = await db.Child("users").GetValueAsync();
        
        foreach(var keys in res.Children) 
        {

            dataLs
                .Add(
                    (keys.Key.ToString(), 
                    JsonUtility.FromJson<FirebaseUserData>
                    (keys.Child("UserData").Value.ToString())
                    ));

        }

        return dataLs;

    }

    public async void SendFriendReq(string postUserKey)
    {

        var res = await db.Child("users").Child(postUserKey).Child("FriendReq").GetValueAsync();
        var data = JsonUtility.FromJson<FirebaseFriendReqData>(res.Value.ToString());

        if(data != null)
        {

            data.reqs.Add(user.UserId);

        }

    }

    public async Task<FirebaseFriendReqData> GetFriendReq()
    {

        var res = await db.Child("users").Child(user.UserId).Child("FriendReq").GetValueAsync();

        return JsonUtility.FromJson<FirebaseFriendReqData>(res.Value.ToString());

    }

    private void OnDestroy()
    {
        
        if(db != null && DeckManager.Instance != null)
        {

            SaveUserData();

        }

    }

}