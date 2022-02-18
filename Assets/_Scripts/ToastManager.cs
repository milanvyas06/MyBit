using System;
using System.Collections;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance;

    private string message;

    private AndroidJavaObject javaObj;

    private AndroidJavaClass javaClass;

    private AndroidJavaObject javaObj1;

    public int processNumber;

    public string effectName;

    public string musicPathA;

    public bool shouldLoadVideo;

    public bool isResetedEverythings;


    public void showToastOnUiThread(string msg)
    {
        this.message = msg;
        javaObj.Call("runOnUiThread", new AndroidJavaRunnable(CallShow));
    }


    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            javaObj = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
            javaObj1 = javaObj.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
        }
        resetEverything();
    }


    private void resetEverything()
    {
        if (instance != null)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        isResetedEverythings = true;
        shouldLoadVideo = false;
        effectName = "0";
        instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }




    private void CallShow()
    {
        AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.String", message);
        AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("makeText", new object[3]
        {
            javaObj1,
            androidJavaObject,
            androidJavaClass.GetStatic<int>("LENGTH_SHORT")
        });
        androidJavaObject2.Call("show");
    }

    public void ToAndroidClipBoard(string msg)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log(msg);
        }
    }

}
