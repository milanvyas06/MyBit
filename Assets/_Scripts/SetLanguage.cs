using System;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguage : MonoBehaviour
{
    public static SetLanguage instance;

    public List<LanguageManager> langManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void ChangeLanguage(string str)
    {
        try
        {
            foreach (LanguageManager item in langManager)
            {
                item.SetLanguages(str);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Got an error on ChangeLanguage " + ex.Message);
        }
    }
}
