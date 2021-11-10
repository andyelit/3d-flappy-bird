using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public string sceneName;
    private Button m_Button;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
