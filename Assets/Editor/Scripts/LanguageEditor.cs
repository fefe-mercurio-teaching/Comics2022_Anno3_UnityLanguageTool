using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LanguageEditor : EditorWindow
{
    [MenuItem("Tools/Language Editor")]
    private static void OpenWindow()
    {
        GetWindow<LanguageEditor>();
    }

    private Vector2 scrollPosition;
    private string[] languages;
    private string[] languageLabels;
    private int selectedLanguageIndex;

    private void GetAllLanguages()
    {
        languages = AssetDatabase.FindAssets("t: Language");
        languageLabels = new string[languages.Length];

        for (int i = 0; i < languages.Length; i++)
        {
            languages[i] = AssetDatabase.GUIDToAssetPath(languages[i]);
            languageLabels[i] = Path.GetFileName(languages[i]);
        }
    }

    private void OnGUI()
    {
        titleContent = new GUIContent("Language Editor");

        if (GUILayout.Button("New Language"))
        {
            NewLanguage();
        }
        
        GUILayout.Space(20f);
        
        GetAllLanguages();

        if (languages.Length == 0)
        {
            EditorGUILayout.HelpBox("No language found", MessageType.Error);
            return;
        }
        
        selectedLanguageIndex = EditorGUILayout.Popup("Language", selectedLanguageIndex, 
            languageLabels);
        GUILayout.Label(languages[selectedLanguageIndex]);

        Language language = AssetDatabase.LoadAssetAtPath<Language>(languages[selectedLanguageIndex]);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < language.strings.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("e", EditorStyles.miniButtonLeft, GUILayout.Width(25f)))
            {
                EditLanguageStringWindow editWindow = GetWindow<EditLanguageStringWindow>();

                editWindow.stringIndex = language.strings[i].index;
                editWindow.stringContent = language.strings[i].value;
                
                editWindow.ShowModalUtility();

                if (editWindow.confirm)
                {
                    language.strings[i].value = editWindow.stringContent;
                    EditorUtility.SetDirty(language);
                }
            }

            if (GUILayout.Button("-", EditorStyles.miniButtonRight,GUILayout.Width(25f)))
            {
                if (EditorUtility.DisplayDialog("Confirm",
                    $"Do you really want to remove the string {language.strings[i].index}?", 
                    "Yes", 
                    "No"))
                {
                    RemoveString(language.strings[i].index);
                    break;
                }
            }
            
            EditorGUILayout.LabelField(language.strings[i].index, language.strings[i].value);
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("New String"))
        {
            LanguageStringEditor stringEditor = EditorWindow.GetWindow<LanguageStringEditor>();
            stringEditor.ShowModalUtility();

            if (!string.IsNullOrWhiteSpace(stringEditor.stringIndex))
            {
                string newStringIndex = stringEditor.stringIndex.ToUpper();

                if (language.strings.Exists(s => s.index == newStringIndex))
                {
                    EditorUtility.DisplayDialog("Error", 
                        $"String {newStringIndex} already exists", "OK");
                }
                else
                {
                    AddString(newStringIndex); // < ---
                }
            }
        }
    }

    private void AddString(string stringIndex)
    {
        foreach (string languagePath in languages)
        {
            Language language = AssetDatabase.LoadAssetAtPath<Language>(languagePath);

            Language.TranslatedString newString = new Language.TranslatedString()
            {
                index = stringIndex,
                value = ""
            };

            language.strings.Add(newString);
            
            EditorUtility.SetDirty(language);
        }
    }

    private void NewLanguage()
    {
        string path = EditorUtility.SaveFilePanelInProject("New Language",
            "language", "asset", "New language location");

        if (!string.IsNullOrWhiteSpace(path))
        {
            Language newLanguage = ScriptableObject.CreateInstance<Language>();
            Language currentLanguage = AssetDatabase.LoadAssetAtPath<Language>(languages[selectedLanguageIndex]);
            
            AssetDatabase.CreateAsset(newLanguage, path);
            
            newLanguage.strings.AddRange(currentLanguage.strings);
            
            EditorUtility.SetDirty(newLanguage);
        }
    }

    private void RemoveString(string stringIndex)
    {
        foreach (string languagePath in languages)
        {
            Language language = AssetDatabase.LoadAssetAtPath<Language>(languagePath);
            language.strings.RemoveAll(s => s.index == stringIndex);
            
            EditorUtility.SetDirty(language);
        }
    }
}
