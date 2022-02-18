using UnityEngine;
using UnityEngine.UI;

public class BackGroundCanvas : MonoBehaviour
{
    public Image img;

    [Space]
    public Sprite spSmallSplash;

    public Sprite spMediumSplash;

    public Sprite spLargeSplash;

    private void Awake()
    {
        if (Screen.height < 1280)
        {
            base.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        }
        if (Screen.height > 1280)
        {
            base.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        }
        if (Screen.width < 720)
        {
            img.sprite = spSmallSplash;
        }
        if (Screen.width >= 720 && Screen.width < 1080)
        {
            img.sprite = spMediumSplash;
        }
        if (Screen.width >= 1080)
        {
            img.sprite = spLargeSplash;
        }
    }

}
