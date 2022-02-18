using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditTextHelper : MonoBehaviour
{
    public static EditTextHelper instance;

    public Text txt;

    public TextData txtData;

    public TextMeshProUGUI textMeshPro;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetText();
    }

    public void LoadText(List<string> lstOfTxt)
    {
        ParticleData component = SettingManager.instance.selectedParticleTemplate.GetComponent<ParticleData>();
        int num = 0;
        foreach (string item in lstOfTxt)
        {
            for (int i = 0; i < component._Texts.Length; i++)
            {
                for (int j = 0; j < component._Texts[i]._Text.Length; j++)
                {
                    component._Texts[i]._Text[j].text = lstOfTxt[i];
                }
            }
            num++;
        }
        int num2 = 0;
        foreach (string item2 in lstOfTxt)
        {
            for (int k = 0; k < component._Texts.Length; k++)
            {
                for (int l = 0; l < component._Texts[k]._TextMeshPro.Length; l++)
                {
                    component._Texts[k]._TextMeshPro[l].text = lstOfTxt[k];
                }
            }
            num2++;
        }
        if (SettingManager.instance.isPlaying)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
    }

    public void SetText()
    {
    }

    public void EditTextEntered(string json)
    {
        Debug.Log(json);
        //txt.text = json;
        if (!(string.IsNullOrEmpty(json)))
        {
            UnityEngine.PlayerPrefs.SetString("LastEditedText", json);
        }

        TextData textData = new TextData();
        textData.data = new List<string>();
        textData.data.Add(json);

        ExportManager.instance._myTextData = textData;
        LoadText(ExportManager.instance._myTextData.data);
    }

}
