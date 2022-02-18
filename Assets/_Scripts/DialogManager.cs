using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public GameObject rateUsPanel;
    public GameObject renderingDialogWithoutAd;

    public GameObject adLoadingPanel;
    public Slider downloaderSlide;
    public Text prTxt;
    public Text fps;

    private const float TOTALAUDIOLENGTH = 30f;

    private float currentTime;
    private bool shouldShowRateUsDialog;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        try
        {

            if (UnityEngine.PlayerPrefs.HasKey("ShouldShowRateUs"))
            {
                int id = UnityEngine.PlayerPrefs.GetInt("ShouldShowRateUs");

                shouldShowRateUsDialog = id == 11 ? true : false;
            }
            else
            {
                UnityEngine.PlayerPrefs.SetInt("ShouldShowRateUs", 11);
                shouldShowRateUsDialog = true;
            }
            downloaderSlide.value = 0;
            currentTime = 0;
        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on Dialog Manager " + ex.Message);
        }
    }


    private void Update()
    {
        try
        {


            fps.text = "FPS :- " + (1.0f / Time.deltaTime).ToString();
            if (downloaderSlide != null && ApplyExportSetting.instance.audioSource.isPlaying)
            {
                currentTime += Time.deltaTime;
                float percentage = (currentTime * 100 / ExportManager.instance.selectionAudioClip.length);
                float prInZero = (currentTime * 100 / ExportManager.instance.selectionAudioClip.length) / 100;

                if (percentage <= 100f && isInProcess())
                {
                    downloaderSlide.value = prInZero;
                    prTxt.text = String.Format("{0:P0}", downloaderSlide.value);
                }
                else
                {
                    if (percentage > 10f)
                    {
                        downloaderSlide.value = 100f;
                        prTxt.text = String.Format("{0:P0}", downloaderSlide.value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on Dialog manager :- " + ex.Message);
        }
    }

    private bool isInProcess()
    {
        return Application.platform == RuntimePlatform.Android ? CaptureController.instance.inProgress : true;

    }



    private void StopCapturing(String ads)
    {
        if (shouldShowRateUsDialog)
        {
            renderingDialogWithoutAd.SetActive(false);
            ADManager.Instance.ShowGoogleInterstitialAds();
            rateUsPanel.SetActive(true);
        }
        else
        {
            CaptureController.instance.PlayerScreen("ads");
        }
    }

    public void OnClickLater()
    {
        rateUsPanel.SetActive(false);
        CaptureController.instance.PlayerScreen("noads");
    }

    public void OnRateUsClick()
    {
        UnityEngine.PlayerPrefs.SetInt("ShouldShowRateUs", 10);

        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        CaptureController.instance.PlayerScreenRate();
    }


}
