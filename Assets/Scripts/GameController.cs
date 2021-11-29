using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public Language currentLanguage;

    private void Awake()
    {
        instance = this;
    }
}
