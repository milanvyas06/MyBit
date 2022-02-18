using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    [Space]
    public Text txt;

    public Language lang;

    [Space]
    public bool shouldAdd;

    private void Start()
    {
        if (shouldAdd)
        {
            SetLanguage.instance.langManager.Add(this);
        }
    }

    public void SetLanguages(string selectedLang)
    {
        switch (selectedLang)
        {
            case "English":
                txt.text = lang.English;
                break;
            case "Telugu":
                txt.text = lang.Telugu;
                break;
            case "Tamil":
                txt.text = lang.Tamil;
                break;
            case "Malayalam":
                txt.text = lang.Malayalam;
                break;
            case "Kannada":
                txt.text = lang.Kannada;
                break;
            case "Hindi":
                txt.text = lang.Hindi;
                break;
            case "Gujarati":
                txt.text = lang.Gujarati;
                break;
            case "Marathi":
                txt.text = lang.Marathi;
                break;
            case "Punjabi":
                txt.text = lang.Punjabi;
                break;
            case "Arabic":
                txt.text = lang.Arabic;
                break;
        }
    }

}