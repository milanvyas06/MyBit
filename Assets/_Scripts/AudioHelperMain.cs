using System;
using System.Collections;
using UnityEngine;

public class AudioHelperMain : MonoBehaviour
{
    public static AudioHelperMain instance;

    public AudioClip audioClip;

    public AudioClip audioClip1;

    public AudioSource audioSourceAndroid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ExportManager.instance.selectionAudioClip = audioClip;
        if (ExportManager.instance.audioPath != string.Empty)
        {
            ExportManager.instance.audioPath = string.Empty;
        }
        if (Time.timeScale == 0f)
        {
            Debug.Log(" Time Scale is 0");
        }
    }

    public void OnSelectMusic(string musicPath)
    {
        ToastManager.instance.effectName = "1";
        ToastManager.instance.musicPathA = musicPath;
        ToastManager.instance.ToAndroidClipBoard(musicPath);
        StartCoroutine(LoadMusicFromGivenPath(musicPath));
    }

    private IEnumerator LoadMusicFromGivenPath(string path)
    {
        audioSourceAndroid.Pause();
        WWW wWW = new WWW("file://" + path);
        while (!wWW.isDone)
        {
            yield return null;
        }
        try
        {
            audioClip = wWW.GetAudioClip(threeD: false);
            audioSourceAndroid.clip = audioClip;
            audioSourceAndroid.Play();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            ToastManager.instance.ToAndroidClipBoard("LoadMusic EX" + ex.Message);
        }
        ExportManager.instance.selectionAudioClip = audioClip;
        ExportManager.instance.audioPath = path;
        if (ExportManager.instance.selectedMultipleImage)
        {
            TransactionManager.instance.RestartTransactionInPreview();
            TransactionManager.instance.RestartTransactionInPrafab();
        }
    }
}
