using System.Collections.Generic;
using UnityEngine;

public class ImageSetupManager : MonoBehaviour
{
    public List<ImageData> lstOFImage = new List<ImageData>();

    [HideInInspector]
    public ImagesManager imgManager;

    [HideInInspector]
    public bool isItLastImage;

    [HideInInspector]
    public bool ignore;

    [HideInInspector]
    public bool isSetupDone;

    [Space]
    public Animator animator;

    [Space]
    public AudioSyncWithColor[] audioSyncWithColor;

    public AudioSyncWithImageFillAmount[] audioSyncWithImageFillAmount;

    public AudioSyncWithLight[] audioSyncWithLight;

    public AudioSyncWithGivenAxis[] audioSyncWithGivenAxis;

    public AudioSyncWithRandomLocalPos[] audioSyncWithRandomLocalPos;

    public AudioSyncWithLensFlareBright[] audioSyncWithLensFlareBright;

    public AudioSyncWithMtone[] audioSyncWithMtone;

    public void Update()
    {
        if (!isSetupDone || isItLastImage)
        {
            return;
        }
        if (imgManager.calculateDistance <= 0f)
        {
            if (imgManager.CurrentImage >= (float)(imgManager._TotalImages - 1))
            {
                isItLastImage = true;
            }
            else
            {
                imgManager.calculateDistance = imgManager.distance;
                string name = "Image" + imgManager.CurrentImage;
                animator.SetBool(name, value: true);
                imgManager.CurrentImage += 1f;
            }
        }
        else
        {
            if (Time.timeScale == 0f)
            {
                imgManager.calculateDistance = 0f;
            }
            imgManager.calculateDistance -= Time.deltaTime;
        }
        imgManager.Counter += Time.deltaTime;
    }

    public void SetUpAnimationData(List<float> lsfOfFloat)
    {
        try
        {
            imgManager.ResetImageManager();
            isItLastImage = false;
            imgManager._TotalImages = (int)lsfOfFloat[0];
            imgManager.clipLength = lsfOfFloat[1];
            imgManager.distance = lsfOfFloat[2];
            imgManager.calculateDistance = imgManager.distance;
            isSetupDone = true;

        }
        catch (System.Exception ex)
        {
            Debug.Log("Found an error on SetUpAnimationData : " + ex.Message);
        }

    }

}
