using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public static ImageLoader instance;

    public List<string> paths;

    public List<Sprite> pathSprites;

    public Text txt; //700 Return asset down side

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        string text = "E:/Free Pic/XYZ/1.jpg?E:/Free Pic/XYZ/2.jpg?E:/Free Pic/XYZ/3.jpg";
    }


    /// <summary>
    /// CALL BACK Funtion
    /// </summary>
    public void DefaultImg()
    {
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
    }

    /// <summary>
    /// Helper for single image change
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private IEnumerator LoadImage(string str)
    {
        byte[] data = null;
        Texture2D texture2D = null;
        try
        {
            texture2D = new Texture2D(128, 128, TextureFormat.RGB24, false);
            data = File.ReadAllBytes(str);
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 1 : " + ex.Message);
        }
        yield return new WaitForSeconds(0.1f);
        try
        {
            texture2D.LoadImage(data);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
            LoadPhoto(null);// WEIRD BUG FIX FOR IMAGE CAUSE TO DISSPEAR WHILE CHANGE IT ON RUNTIME
            LoadPhoto(sprite);
        }
        catch (Exception ex2)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 2 : " + ex2.Message);
        }
        AudioHelperMain.instance.audioSourceAndroid.Play();
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
    }


    /// <summary>
    /// Helper for transcation image change
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadImageFromGivenPath(string path)
    {
        byte[] data = null;
        Texture2D texture2D = null;
        try
        {
            texture2D = new Texture2D(128, 128, TextureFormat.RGB24, false);
            data = File.ReadAllBytes(path);
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 1 : " + ex.Message);
        }
        yield return new WaitForSeconds(0.1f);
        try
        {
            texture2D.LoadImage(data);
            Sprite item = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
            pathSprites.Add(item);
        }
        catch (Exception ex2)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 2 : " + ex2.Message);
        }
    }

    public void GetSelectedImagePath(string imgPath)
    {
        ExportManager.instance.selectedMultipleImage = false;
        if (SettingManager.instance.selectedTranscation == null)
        {
            if (SettingManager.instance.selectedPrefabTheme == null)
            {
                int[] array = new int[4]
                {
                    0,
                    1,
                    2,
                    4
                };
                int num = UnityEngine.Random.Range(0, array.Length);
                ThemeManager.instance.LoadPrefabTheme(ThemeManager.instance.lstOFThemePrefab[array[num]]._Prefab, ThemeManager.instance.totalTheme[array[num]]);
            }
        }
        else
        {
            UnityEngine.Object.Destroy(SettingManager.instance.selectedTranscation);
            if (SettingManager.instance.selectedPrefabTheme == null)
            {
                int[] array2 = new int[4]
                {
                    0,
                    1,
                    2,
                    4
                };
                int num2 = UnityEngine.Random.Range(0, array2.Length);
                ThemeManager.instance.LoadPrefabTheme(ThemeManager.instance.lstOFThemePrefab[array2[num2]]._Prefab, ThemeManager.instance.totalTheme[array2[num2]]);
            }
        }
        SettingManager.instance.themes.SetActive(value: true);
        SettingManager.instance.transactions.SetActive(value: false);
        if (SettingManager.instance.isPlaying)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
        StartCoroutine(LoadImage(imgPath));
        SettingManager.instance.returnAssetLoading.SetActive(value: false);
    }

    /// <summary>
    /// CALLBACK FUNCTION FOR LOAD MULTIPLE IMAGES
    /// </summary>
    /// <param name="imagesPath"></param>
    public void OnSelectMultipleImg(string imagesPath)
    {
        ToastManager.instance.ToAndroidClipBoard(imagesPath);
        Time.timeScale = 1f;
        string[] imagePaths = imagesPath.Split('?');
        int totalImages = imagePaths.Length;
        List<string> lstOfPaths = new List<string>();
        int current = 0;
        for (int i = 0; i < 6; i++)
        {
            if (current >= totalImages)
            {
                current = 0;
            }
            lstOfPaths.Add(imagePaths[current]);
            current++;
        }
        ExportManager.instance.selectedMultipleImage = true;
        SettingManager.instance.themes.SetActive(value: false);
        SettingManager.instance.transactions.SetActive(value: true);
        try
        {
            if (SettingManager.instance.selectedPrefabTheme == null)
            {
                if (SettingManager.instance.selectedTranscation == null)
                {
                    TransactionManager.instance.OnClickToTransaction(TransactionManager.instance.lstOfTranscation[0].Prefab, TransactionManager.instance.themeContainers[0]);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(SettingManager.instance.selectedPrefabTheme);
                if (SettingManager.instance.selectedTranscation == null)
                {
                    TransactionManager.instance.OnClickToTransaction(TransactionManager.instance.lstOfTranscation[0].Prefab, TransactionManager.instance.themeContainers[0]);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            txt.text = ex.ToString();
        }
        ExportManager.instance.id = new List<Sprite>(0);
        pathSprites = new List<Sprite>(0);
        for (int j = 0; j < lstOfPaths.Count; j++)
        {
            StartCoroutine(LoadImageFromGivenPath(lstOfPaths[j]));
        }
        StartCoroutine(setImageIntpTranscationManager(TransactionManager.instance.ManageTransaction(lstOfPaths.Count, AudioHelperMain.instance.audioClip)));
        StartCoroutine(SetImageInsideMainPreview(TransactionManager.instance.ManageTransaction(lstOfPaths.Count, AudioHelperMain.instance.audioClip)));
    }


    /// <summary>
    /// Change Image in Preview transcation
    /// </summary>
    /// <param name="lstOfFloat"> Anim Data </param>
    /// <returns></returns>
    private IEnumerator setImageIntpTranscationManager(List<float> lstOfFloat)
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < TransactionManager.instance.prefabEffectDetails.Count; i++)
        {
            for (int j = 0; j < TransactionManager.instance.prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().lstOFImage.Count; j++)
            {
                for (int k = 0; k < TransactionManager.instance.prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().lstOFImage[j]._Image.Count; k++)
                {
                    if (j < pathSprites.Count)
                    {
                        TransactionManager.instance.prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().lstOFImage[j]._Image[k].sprite = pathSprites[j];
                    }
                }
                TransactionManager.instance.prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().SetUpAnimationData(lstOfFloat);
            }
        }
    }

    /// <summary>
    /// Load image in preview canvas
    /// </summary>
    /// <param name="sprite"></param>
    public void LoadPhoto(Sprite sprite)
    {
        ExportManager.instance.selectedImageSprite = sprite;
        try
        {
            ThemeSubDetails component = SettingManager.instance.selectedPrefabTheme.GetComponent<ThemeSubDetails>();
            for (int i = 0; i < component.lstOfUserImage.Length; i++)
            {
                for (int j = 0; j < component.lstOfUserImage[i]._Images.Length; j++)
                {
                    component.lstOfUserImage[i]._Images[j].sprite = sprite;
                    // WORK AROUND WEIRD BUG FOR IMAGE CUASE TO DISSPEAR
                    component.lstOfUserImage[i]._Images[j].transform.gameObject.SetActive(false);
                    component.lstOfUserImage[i]._Images[j].transform.gameObject.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 3 : " + ex.Message);
        }
        LoadImagesInPreview(sprite);
    }

    /// <summary>
    /// Change image in main preview
    /// </summary>
    /// <param name="lstOfFloat"> Anim Data </param>
    /// <returns></returns>
    private IEnumerator SetImageInsideMainPreview(List<float> lstOfFloat)
    {
        yield return new WaitForSeconds(0.2f);
        try
        {
            if (SettingManager.instance.selectedTranscation != null)
            {
                ImageSetupManager component = SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>();
                for (int i = 0; i < component.lstOFImage.Count; i++)
                {
                    for (int j = 0; j < component.lstOFImage[i]._Image.Count; j++)
                    {
                        if (i < pathSprites.Count)
                        {
                            component.lstOFImage[i]._Image[j].sprite = pathSprites[i];
                        }
                    }
                    component.SetUpAnimationData(lstOfFloat);
                }
            }
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard(ex.ToString());
            txt.text = ex.ToString();
        }
        if (pathSprites == null)
        {
            txt.text = "Sprite Could not be loaded";
        }
        ExportManager.instance.id = pathSprites;
        SettingManager.instance._PreviewLoadingPanel.SetActive(value: false);
        AudioHelperMain.instance.audioSourceAndroid.Play();
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
        if (ExportManager.instance.selectedMultipleImage)
        {
            TransactionManager.instance.RestartTransactionInPreview();
            TransactionManager.instance.RestartTransactionInPrafab();
        }
    }

    /// <summary>
    /// Load theme preview
    /// </summary>
    /// <param name="sprite"></param>
    public void LoadImagesInPreview(Sprite sprite)
    {
        try
        {
            foreach (ThemeSubDetails item in ThemeManager.instance.prefabEffectDetails)
            {
                for (int i = 0; i < item.lstOfUserImage.Length; i++)
                {
                    for (int j = 0; j < item.lstOfUserImage[i]._Images.Length; j++)
                    {
                        item.lstOfUserImage[i]._Images[j].sprite = sprite;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("Err 4 : " + ex.Message);
        }
        SettingManager.instance._PreviewLoadingPanel.SetActive(value: false);
        AudioHelperMain.instance.audioSourceAndroid.Play();
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
    }

    public void NothingIsChanged()
    {
        if (ExportManager.instance.selectedMultipleImage)
        {
            TransactionManager.instance.RestartTransactionInPreview();
            TransactionManager.instance.RestartTransactionInPrafab();
        }
        SettingManager.instance._PreviewLoadingPanel.SetActive(value: false);
        AudioHelperMain.instance.audioSourceAndroid.Play();
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
    }

}
