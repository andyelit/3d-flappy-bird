using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    public GameObject pfPipe;

    public SoundAudioClip[] soundAudioClipArray;

    string jsonURL = "https://drive.google.com/uc?export=download&id=1Rg_QKGNh5S8t6ziMnkwT4GKhEqtFpK2Q";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    public struct Data
    {
        public string pipeBundleURL;
    }
}
