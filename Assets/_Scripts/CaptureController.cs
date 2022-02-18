using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CaptureController : MonoBehaviour
{
    public static CaptureController instance;

    public int videoWidth;

    public int videoHeight;

    public int videoFrameRate = 30;
    public AudioSource recordableAudio;
    public Camera cam;

    public event Action OnStopCapturing;


    private IMediaRecorder videoRecorder;
    private IClock recordingClock;
    private CameraInput cameraInput;
    private AudioInput audioInput;
    private string videopath;
    private string videoDir;
    private string filepath;

    private bool audiofocus = true;

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    static AndroidJavaObject activityContext;

    AudioFocusListener m_FocusListener;

    public bool inProgress
    {
        get;
        private set;
    }

    class AudioFocusListener : AndroidJavaProxy
    {
        public AudioFocusListener() : base("android.media.AudioManager$OnAudioFocusChangeListener") { }

        public bool m_HasAudioFocus = true;

        public bool HasAudioFocus { get { return m_HasAudioFocus; } }

        public void onAudioFocusChange(int focus)
        {
            m_HasAudioFocus = (focus >= 0);
            CaptureController.instance.audiofocus = m_HasAudioFocus;
            if (CaptureController.instance.inProgress)
            {
                if (!m_HasAudioFocus)
                {
                    ExportManager.instance.isInterupt = true;
                    _pluginInstance.CallStatic("setRfailedtrue");
                    SceneManager.LoadScene("_previewScene");
                }
            }
            Debug.Log("sound instance " + m_HasAudioFocus);
        }
    }

    private void Awake()
    {
        instance = this;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (m_FocusListener == null)
                m_FocusListener = new AudioFocusListener();

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject audioManager = activity.Call<AndroidJavaObject>("getSystemService", "audio");

            audioManager.Call<Int32>("requestAudioFocus", m_FocusListener, 3, 1);
        }

        if (ExportManager.instance.exportResolution == 360)
        {
            videoWidth = 360;
            videoHeight = 640;
            Screen.SetResolution(360, 640, fullscreen: true);
        }
        else if (ExportManager.instance.exportResolution == 480)
        {
            videoWidth = 480;
            videoHeight = 854;
            Screen.SetResolution(480, 854, fullscreen: true);
        }
        else if (ExportManager.instance.exportResolution == 720)
        {
            videoWidth = 720;
            videoHeight = 1280;
            Screen.SetResolution(720, 1280, fullscreen: true);
        }
        try
        {

        }
        catch (Exception ex)
        {
            Debug.Log("Err : " + ex.Message);
            ToastManager.instance.ToAndroidClipBoard("Int : " + ex.Message);
        }
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            _pluginClass = new AndroidJavaClass(PackageManager.PLUGINPACKAGENAME);
            _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("instance");
            _pluginInstance.CallStatic("setContext", activityContext);

            inProgress = false;
            videoDir = _pluginInstance.CallStatic<string>("getDirectoryDCIM");
        }
    }

    public void StartCapturing()
    {
        ADManager.Instance.LoadGoogleInterstitialAd();
        try
        {
            var recordingDirectory = videoDir;
            var recordingFilename = string.Format("Feel_The_Beat_{0}.mp4", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));

            videopath = Path.Combine(recordingDirectory, recordingFilename);

            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                videopath,
                30,
                AudioSettings.outputSampleRate,
                (int)AudioSettings.speakerMode,
                OnReplay
            );

            cameraInput = new CameraInput(videoRecorder, recordingClock, cam);
            audioInput = new AudioInput(videoRecorder, recordingClock, recordableAudio);
            inProgress = true;

        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on StartCapturing() : " + ex.Message);
        }

    }

    private void OnReplay(string path)
    {
        _pluginInstance.CallStatic("vidFinished", path);

    }

    public async void StopCapturing()
    {

        try
        {
            inProgress = false;
            Debug.Log("Stopped Recoding 1...");
            audioInput.Dispose();
            cameraInput.Dispose();
            videoRecorder.Dispose();

            recordableAudio.mute = true;
            Debug.Log("Video Recording is stopped.!");
            OnStopCapturing?.Invoke();
            inProgress = false;

        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on : Stop Capturing" + ex.Message);
        }
    }

    private void OnDestroy()
    {
        recordableAudio?.Stop();
        Microphone.End(null);
    }


    public void PlayerScreen(String ads)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Unity Call");
            _pluginInstance.Call("playerscreen", new string[] { videopath, ads });
            SceneManager.LoadScene("_PreviewScreen");
        }
    }

    public void PlayerScreenRate()
    {
        if (Application.platform == RuntimePlatform.Android) StartCoroutine(PlayerScreenRateUs("noads"));
    }
    public IEnumerator PlayerScreenRateUs(String ads)
    {
        yield return new WaitForSeconds(1f);
        _pluginInstance.Call("playerscreen", new string[] { filepath, ads });
        SceneManager.LoadScene("_PreviewScreen");

    }
}
