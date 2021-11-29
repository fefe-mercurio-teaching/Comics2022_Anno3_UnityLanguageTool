using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslatedTextMesh : MonoBehaviour
{
    public string stringIndex;
    private string lastIndex = string.Empty;

    private void Update()
    {
        if (GameController.instance == null) // Singleton
        {
            return;
        }

        if (stringIndex != lastIndex)
        {
            lastIndex = stringIndex;
            
            TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
            textMesh.text = 
                GameController.instance.currentLanguage.GetString(stringIndex);
        }
    }

    private void OnEnable()
    {
        
    }
}
