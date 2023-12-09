using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class TowerObj
{

    public string key;
    public NetworkObject obj;
    public Sprite sprite;
    public int cost;
    public string towerName;
    public int price;
    [TextArea] public string expText;

}

[CreateAssetMenu(menuName = "SO/Tower/TowerList")]
public class TowerListSO : ScriptableObject
{

    public List<TowerObj> lists;

}
