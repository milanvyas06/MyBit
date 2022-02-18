using UnityEngine;

public class ScreenShotCapturer : MonoBehaviour
{
    public ResolutionSS resolution = ResolutionSS.asperResoultion;

    public KeyCode keycod = KeyCode.G;

    public int x = 1;

    private string tempText;

    private void Update()
    {
        if (Input.GetKeyDown(keycod))
        {
            Screen.SetResolution(640, 1136, fullscreen: false);
            tempText = string.Empty + Screen.width * x + "X" + Screen.height * x;
            ScreenCapture.CaptureScreenshot("ScreenShot-" + tempText + "-" + UnityEngine.PlayerPrefs.GetInt("number", 0) + "size_" + (int)resolution + "_.png", (int)resolution * x);
            UnityEngine.PlayerPrefs.SetInt("number", UnityEngine.PlayerPrefs.GetInt("number", 0) + 1);
        }
    }

}
