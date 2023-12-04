using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUIController : MonoBehaviour
{

    [Header("로그인")]
    [SerializeField] private TMP_InputField loginEmailField;
    [SerializeField] private TMP_InputField loginPasswordField;
    [SerializeField] private TMP_Text loginErrorText;

    [Header("회원가입")]
    [SerializeField] private TMP_InputField signUpEmailField;
    [SerializeField] private TMP_InputField signUpPasswordField;
    [SerializeField] private TMP_InputField signUpUserNameField;
    [SerializeField] private TMP_Text signUpErrorText;

    private void Awake()
    {

        loginErrorText.text = "";
        signUpErrorText.text = "";

    }

    public void Login()
    {

        loginErrorText.text = "";
        FirebaseManager.Instance.Login(loginEmailField.text, loginPasswordField.text, HandleLoginEnd);

    }

    public void SignUp()
    {

        signUpErrorText.text = "";
        FirebaseManager.Instance.CreateAccount(signUpEmailField.text, signUpPasswordField.text, signUpUserNameField.text, HandleSignUpEnd);

    }

    private void HandleLoginEnd(bool success, FirebaseUser user)
    {

        if(success)
        {

            FirebaseManager.Instance.LoadUserdata();
            SceneManager.LoadScene(SceneList.MenuScene);

        }
        else
        {

            loginErrorText.text = "다시 시도해주세요!";

        }

    }

    private void HandleSignUpEnd(bool success)
    {

        if (success)
        {

            SceneManager.LoadScene(SceneList.MenuScene);

        }
        else
        {

            signUpErrorText.text = "다시 시도해주세요!";

        }

    }

}
