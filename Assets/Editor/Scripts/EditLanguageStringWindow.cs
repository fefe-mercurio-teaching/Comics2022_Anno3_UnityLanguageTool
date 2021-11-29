using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditLanguageStringWindow : EditorWindow
{
    public string stringIndex;
    public string stringContent;
    public bool confirm;
    
    private void OnGUI()
    {
        titleContent = new GUIContent("Edit string");
        
        GUILayout.Label("Index");

        GUI.enabled = false;
        EditorGUILayout.TextField(stringIndex);
        GUI.enabled = true;
        
        GUILayout.Label("Content");
        stringContent = EditorGUILayout.TextArea(stringContent, GUILayout.ExpandHeight(true));
        
        GUILayout.Space(25f);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            confirm = true;
            Close();
        }

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        
        GUILayout.EndHorizontal();
    }
}
