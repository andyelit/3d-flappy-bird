using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LoadServerGameAssetsButton : MonoBehaviour
{
    private Button m_Button;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        if (HasConnection())
        {
            GameAssets.GetInstance().LoadGameAssetsFromServer();
            m_Button.interactable = false;
            m_Button.transform.GetChild(0).GetComponent<Text>().text = "loading...";
        }
        else
            ShowAndroidToastMessage("You need internet connection for to play");
    }

    public bool HasConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = new WebClient().OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
