using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public static DeckManager Instance { get; private set; }

    public List<string> DeckLs { get; set; } = new();
    public List<string> AbleTowerLs { get; set; } = new();

    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
        Instance = this;

    }

}
