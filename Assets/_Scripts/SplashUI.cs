
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashUI : MonoBehaviour
{

    public GameObject waitPanel;

    public GameObject updatePanel;

    public GameObject noIConnection;

    public GameObject permissionLoader;

    public Animator splashScreen;

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    static AndroidJavaObject activityContext;
    const string nextSceneName = "_PreviewScreen";

    private AsyncOperation scene;

    private void Start()
    {
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginClass = new AndroidJavaClass(PackageManager.PLUGINPACKAGENAME);
                _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("instance");
                _pluginInstance.CallStatic("setContext", activityContext);
            }
            LoadHome();
        }
        catch (Exception ex)
        {
            Debug.Log("Error Launch " + ex.Message);
        }
    }

    private void LoadHome()
    {
        permissionLoader.SetActive(false);
        AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.READ_EXTERNAL_STORAGE", "android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.RECORD_AUDIO");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted && result[2] == AndroidRuntimePermissions.Permission.Granted)
            {
                permissionLoader.SetActive(false);
                StartCoroutine(CallingLaunchApp());
            }
            else
            {
                permissionLoader.SetActive(true);
                LoadHome();
            }
        }
        else
        {
            StartCoroutine(CallingLaunchApp());
        }

    }

    public void LoadPreviewScene()
    {
        Debug.Log("Received call back from android.!");
        if (Application.platform == RuntimePlatform.Android && ADManager.Instance.isInterGoogleAdLoaded)
        {
            ADManager.Instance.OnInterAdClosed += OnInterAdClosed;
            ADManager.Instance.ShowGoogleInterstitialAds();
        }
        else
        {
            scene.allowSceneActivation = true;
            SceneManager.LoadScene("_PreviewScreen");
            _pluginInstance.CallStatic("StartMain", activityContext);
        }
    }

    private void OnInterAdClosed()
    {
        scene.allowSceneActivation = true;
        ADManager.Instance.OnInterAdClosed -= OnInterAdClosed;
        SceneManager.LoadScene("_PreviewScreen");
        _pluginInstance.CallStatic("StartMain", activityContext);
    }

    private IEnumerator CallingLaunchApp()
    {
        splashScreen.Play("SplashScreen");
        yield return new WaitForSeconds(3.5f);
        //waitPanel.SetActive(value: true);

        scene = SceneManager.LoadSceneAsync("_PreviewScreen");
        scene.allowSceneActivation = false;


        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    _pluginInstance.CallStatic("StartApp", activityContext);
                }));

            }
            catch (Exception ex)
            {
                Debug.Log("Error Launch1 " + ex.Message);
            }
        }
        else
        {
            LoadPreviewScene();
        }
    }

    public void DisplayNoInternetDialog()
    {
        Debug.Log("Calling No internet Dialog.!");
        noIConnection.SetActive(true);
    }

    public void DisplayUpdateAvailable()
    {
        updatePanel.SetActive(true);
    }

    public void OnUpdateClick()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void OnClickRetry()
    {
        noIConnection.SetActive(false);
        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.CallStatic("StartMain", activityContext);
        }));
    }

    public void OnLaterClick()
    {
        HideAllDialog();
        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.CallStatic("ExecuteTokenAPI");
        }));
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private void HideAllDialog()
    {
        if (noIConnection.activeSelf) noIConnection.SetActive(false);
        if (updatePanel.activeSelf) updatePanel.SetActive(false);
    }
}
