using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundData
{

    public AudioClip clip;
    public string name;

}

[CreateAssetMenu(menuName = "SO/SoundData")]
public class SoundDataSO : ScriptableObject
{

    public List<SoundData> data = new List<SoundData>();

}