using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddAudio : MonoBehaviour
{
    public static AddAudio instance;

    private void Awake()
    {
        Initilize();
    }


    public void addAudioFail(string str)
    {
        ToastManager.instance.ToAndroidClipBoard("Faild to load audio");
    }

    private void Initilize()
    {
        if (instance != null)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

}
