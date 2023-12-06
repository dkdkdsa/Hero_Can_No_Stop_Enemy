using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

[Serializable]
public class EnemyListData
{

    public string key;
    public float minTime;
    public float weight;
    public NetworkObject netObj;

}

[CreateAssetMenu(menuName = "SO/Enemy/EnemyList")]
public class EnemyListSO : ScriptableObject
{
    
    public List<EnemyListData> list;

}
