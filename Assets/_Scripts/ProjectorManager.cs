using UnityEngine;
using UnityEngine.UI;

public class ProjectorManager : MonoBehaviour
{
    public static ProjectorManager instance;

    public Camera cam;

    public RawImage previewRaw;

    public RawImage unlockPanelRaw;

    public RawImage unlockTransactionRaw;

    public RenderTexture rednTexture;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.instance.IsStoredResolution() == 0)
        {
            if (Screen.width < 240)
            {
                rednTexture = new RenderTexture(Screen.width, Screen.height, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 240 && Screen.width <= 400)
            {
                rednTexture = new RenderTexture(320, 570, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 401 && Screen.width <= 600)
            {
                rednTexture = new RenderTexture(480, 854, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 601 && Screen.width <= 960)
            {
                rednTexture = new RenderTexture(720, 1280, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 961 && Screen.width <= 1260)
            {
                rednTexture = new RenderTexture(1080, 1920, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 1261 && Screen.width <= 1620)
            {
                rednTexture = new RenderTexture(1440, 2560, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (Screen.width > 1620)
            {
                rednTexture = new RenderTexture(Screen.width, Screen.height, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
        }
        else
        {
            if (PlayerPrefs.instance.GetWidth() < 240)
            {
                rednTexture = new RenderTexture(Screen.width, Screen.height, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 240 && PlayerPrefs.instance.GetWidth() <= 400)
            {
                rednTexture = new RenderTexture(320, 570, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 401 && PlayerPrefs.instance.GetWidth() <= 600)
            {
                rednTexture = new RenderTexture(480, 854, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 601 && PlayerPrefs.instance.GetWidth() <= 960)
            {
                rednTexture = new RenderTexture(720, 1280, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 961 && PlayerPrefs.instance.GetWidth() <= 1260)
            {
                rednTexture = new RenderTexture(1080, 1920, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 1261 && PlayerPrefs.instance.GetWidth() <= 1620)
            {
                rednTexture = new RenderTexture(1440, 2560, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
            if (PlayerPrefs.instance.GetWidth() > 1620)
            {
                rednTexture = new RenderTexture(Screen.width, Screen.height, 100);
                cam.targetTexture = rednTexture;
                previewRaw.texture = rednTexture;
                unlockPanelRaw.texture = rednTexture;
                unlockTransactionRaw.texture = rednTexture;

            }
        }
    }

}
