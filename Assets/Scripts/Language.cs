using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language.asset", menuName = "Language")]
public class Language : ScriptableObject
{
    [System.Serializable]
    public class TranslatedString
    {
        public string index;
        public string value;
    }

    public List<TranslatedString> strings = new List<TranslatedString>();

    public string GetString(string stringIndex)
    {
        /*foreach (TranslatedString tString in strings)
        {
            if (tString.index == stringIndex)
            {
                return tString.value;
            }
        }


        return $"{stringIndex} ??";*/
        
        // alternativa
        
        TranslatedString stringFound = strings.Find(s => s.index == stringIndex);
        return stringFound != null ? stringFound.value : $"{stringIndex} ??";
    }
}
