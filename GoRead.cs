using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRead : MonoBehaviour
{
     
    void Start()
    {
         
        LanguageManager.Instance.LoadLang();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Yes No".Translate());
        }
    }

     
}

