using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class TowerObj
{

    public string key;
    public NetworkObject obj;

}

[CreateAssetMenu(menuName = "SO/Tower/TowerList")]
public class TowerListSO : ScriptableObject
{

    public List<TowerObj> lists;

}
