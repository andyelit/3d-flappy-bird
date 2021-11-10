using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button m_Button;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        Application.Quit();
    }
}
