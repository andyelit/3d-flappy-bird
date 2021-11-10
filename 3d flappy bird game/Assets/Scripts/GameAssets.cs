using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance = null;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    public GameObject pfBird;
    public GameObject pfPipe;

    public SoundAudioClip[] soundAudioClipArray;

    string jsonURL = "https://drive.google.com/uc?export=download&id=1v9XMt-_f_c5VAwiUHJEBBZUt6UHFm3st";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void LoadGameAssetsFromServer()
    {
        StartCoroutine(GetData(jsonURL));
    }

    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);

            StartCoroutine(GetAssetBundle(data.GameAssetsBundleURL));
        }

        // Clean up any resources it is using.
        request.Dispose();
    }

    private IEnumerator GetAssetBundle(string URL)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            pfPipe = null;
            pfBird = null;

            pfPipe = bundle.LoadAsset("yellow-pipe") as GameObject;
            pfBird = bundle.LoadAsset("cube-bird") as GameObject;

            SceneManager.LoadScene("Game");
        }
    }


    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    public struct Data
    {
        public string GameAssetsBundleURL;
    }
}
