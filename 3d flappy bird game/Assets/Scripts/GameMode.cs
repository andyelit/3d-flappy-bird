using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public static string mode = "Easy";

    private Dropdown m_Dropdown;

    private void Start()
    {
        m_Dropdown = GetComponent<Dropdown>();

        m_Dropdown.onValueChanged.AddListener(HandleValueChanged);
    }

    private void HandleValueChanged(int indexValue)
    {
        mode = m_Dropdown.options[indexValue].text;
    }
}
