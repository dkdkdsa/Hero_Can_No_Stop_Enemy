using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{

    private Dictionary<string, AudioClip> clipContainer = new();

    public static SoundManager Instance;

    public SoundManager()
    {

        var data = Resources.Load<SoundDataSO>("SoundData");

        foreach (var item in data.data)
        {

            clipContainer.Add(item.name, item.clip);

        }

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {

        Instance = new SoundManager();

    }

    public void PlaySound(string key)
    {

        var obj = new GameObject();
        var sm = obj.AddComponent<AudioSource>();
        sm.clip = clipContainer[key];
        sm.Play();

        FAED.InvokeDelay(() =>
        {

            if (obj == null) return;

            Object.Destroy(obj);

        }, sm.clip.length + 0.1f);

    }

}