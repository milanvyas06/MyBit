using System;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSubDetails : MonoBehaviour
{
    [Serializable]
    public class UserImageList
    {
        public Image[] _Images;
    }

    public UserImageList[] lstOfUserImage;

    public Image img;

    public AudioSyncWithColor[] effectSyncWithColor;

    public AudioSyncWithImageFillAmount[] effectWithFilleImage;

    public AudioSyncWithLight[] effectWithLight;

    public AudioSyncWithGivenAxis[] effectWithGivenAxis;

    public AudioSyncWithRandomLocalPos[] effectObjectVibration;

    public AudioSyncWithLensFlareBright[] effectWithLensFlareBright;

    public AudioSyncWithMtone[] effetcMTone;

    public AudioSyncWithGivenTone[] grayscaleTone;

    [Space]
    public bool isTextUnlocked;
}
