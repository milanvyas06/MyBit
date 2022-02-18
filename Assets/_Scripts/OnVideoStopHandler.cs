using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVideoStopHandler : MonoBehaviour
{
    public static OnVideoStopHandler instance;

    private bool isRecordingON = true;

    public bool isEverythingLoaded;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!ApplyExportSetting.instance.audioSource.isPlaying && isRecordingON)
        {
            StopCapture();
            ExportManager.instance._myTextData.data = new List<string>(0);
            isRecordingON = false;
        }
    }

    public void StopCapture()
    {
        try
        {
            Debug.Log("Calling Stop Capture.!");
            CaptureController.instance.StopCapturing();

            StartCoroutine(ShowFBInterAdToUser());
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("ex stop :" + ex.Message);
        }
    }

    IEnumerator ShowFBInterAdToUser()
    {

        if (Application.platform != RuntimePlatform.Android)
        {
            DialogManager.instance.adLoadingPanel.SetActive(true);
            yield return new WaitForSeconds(1f);

            DialogManager.instance.adLoadingPanel.SetActive(false);
        }
        else
        {
        }

    }


}
