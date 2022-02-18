using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyExportSetting : MonoBehaviour
{
    public static ApplyExportSetting instance;

    public GameObject Theme;

    public GameObject parentGO;

    public AudioSource audioSource;

    private AudioClip audioClip;

    public Text txt;

    public Camera cam;

    public RawImage rawImg;

    public RenderTexture rend360;

    public RenderTexture rend480;

    public RenderTexture rend720;

    public Image layerImage;

    public GameObject waterMarkPanel;

    [HideInInspector]
    public ImageSetupManager imgSetupManager;

    private void Start()
    {
        try
        {

            Time.timeScale = 1f;
            Screen.sleepTimeout = -1;
            instance = this;

            LayerManagement.instance.SetOnExport();

            if (ExportManager.instance.exportResolution == 360)
            {
                cam.targetTexture = rend360;
                rawImg.texture = rend360;
            }
            else if (ExportManager.instance.exportResolution == 480)
            {
                cam.targetTexture = rend480;
                rawImg.texture = rend480;
            }
            else if (ExportManager.instance.exportResolution == 720)
            {
                cam.targetTexture = rend720;
                rawImg.texture = rend720;
            }

            waterMarkPanel.SetActive(ExportManager.instance.shouldRemoveWatermark ? false : true);
            ExportManager.instance.shouldRemoveWatermark = false;

            if (!ExportManager.instance.selectedMultipleImage)
            {
                GameObject generatedTheme = UnityEngine.Object.Instantiate(ExportManager.instance.selectedTheme, new Vector3(0f, 0f, 0f), Quaternion.identity);
                generatedTheme.transform.parent = Theme.transform;
                generatedTheme.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                generatedTheme.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                generatedTheme.transform.localPosition = new Vector3(0f, 0f, 0f);
                generatedTheme.transform.localScale = new Vector3(1f, 1f, 1f);
                if (ExportManager.instance.selectedImageSprite != null)
                {
                    for (int i = 0; i < generatedTheme.transform.GetComponent<ThemeSubDetails>().lstOfUserImage.Length; i++)
                    {
                        for (int j = 0; j < generatedTheme.transform.GetComponent<ThemeSubDetails>().lstOfUserImage[i]._Images.Length; j++)
                        {
                            generatedTheme.transform.GetComponent<ThemeSubDetails>().lstOfUserImage[i]._Images[j].sprite = ExportManager.instance.selectedImageSprite;
                        }
                    }
                }
                ThemeSubDetails component = generatedTheme.transform.GetComponent<ThemeSubDetails>();
                for (int k = 0; k < component.lstOfUserImage.Length; k++)
                {
                    for (int l = 0; l < component.lstOfUserImage[k]._Images.Length; l++)
                    {
                        if (component.isTextUnlocked)
                        {
                            component.lstOfUserImage[k]._Images[l].material = ExportManager.instance.lightMaterial;
                        }
                    }
                }
                ApplyThemeSettings(generatedTheme);
            }
            else
            {
                GameObject generatedTranscation = UnityEngine.Object.Instantiate(ExportManager.instance.transactionData, new Vector3(0f, 0f, 0f), Quaternion.identity);
                generatedTranscation.transform.parent = Theme.transform;
                generatedTranscation.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                generatedTranscation.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                generatedTranscation.transform.localPosition = new Vector3(0f, 0f, 0f);
                generatedTranscation.transform.localScale = new Vector3(1f, 1f, 1f);
                imgSetupManager = generatedTranscation.transform.GetComponent<ImageSetupManager>();
                for (int m = 0; m < imgSetupManager.lstOFImage.Count; m++)
                {
                    for (int n = 0; n < imgSetupManager.lstOFImage[m]._Image.Count; n++)
                    {
                        if (m < ExportManager.instance.id.Count)
                        {
                            generatedTranscation.transform.GetComponent<ImageSetupManager>().lstOFImage[m]._Image[n].sprite = ExportManager.instance.id[m];
                        }
                    }
                }
                ApplyTrasncationSettings(generatedTranscation);
            }
            if (ExportManager.instance.shouldLoadPartileFromBundle)
            {
                StartCoroutine(LoadAssetBundle(ExportManager.instance.particleBundle, ExportManager.instance.prefabName));
            }
            else
            {
                GameObject generatedParticle = UnityEngine.Object.Instantiate(ExportManager.instance.selectedParticle, new Vector3(0f, 0f, 0f), Quaternion.identity);
                generatedParticle.transform.parent = parentGO.transform;
                generatedParticle.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                generatedParticle.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                generatedParticle.transform.localPosition = new Vector3(0f, 0f, 0f);
                generatedParticle.transform.localScale = new Vector3(1f, 1f, 1f);
                ApplyPaticlesSettings(generatedParticle);
                ApplyText(generatedParticle.transform.GetComponent<ParticleData>());
                Debug.Log("MAYUR");
                Camera[] lstOfCamera = generatedParticle.transform.GetComponent<ParticleData>()._Camera;
                foreach (Camera cam in lstOfCamera)
                {
                    if (ExportManager.instance.exportResolution == 360)
                    {
                        cam.targetTexture = rend360;
                    }
                    else if (ExportManager.instance.exportResolution == 480)
                    {
                        cam.targetTexture = rend480;
                    }
                    else if (ExportManager.instance.exportResolution == 720)
                    {
                        cam.targetTexture = rend720;
                    }
                }
            }
            Debug.Log(0);
            if (ExportManager.instance.audioPath == string.Empty)
            {
                Debug.Log(" Found audio path is null.!");
                audioSource.clip = ExportManager.instance.selectionAudioClip;
                audioSource.Play();
                CaptureController.instance.StartCapturing();
                OnVideoStopHandler.instance.isEverythingLoaded = true;
                txt.text = ExportManager.instance.selectionAudioClip.length.ToString();
                imgSetupManager.SetUpAnimationData(ManageTransaction(ExportManager.instance.id.Count, audioSource.clip));
            }
            else
            {
                Debug.Log("ExportController.instance.audioPath is" + ExportManager.instance.audioPath);
                StartCoroutine(LoadAudio(ExportManager.instance.audioPath, imgSetupManager));
            }
            Debug.Log(1);
        }
        catch (Exception ex)
        {
            Debug.Log(" Found an error on Start Method :- " + ex.Message);
        }
    }
    private void ApplyPaticlesSettings(GameObject particle)
    {
        for (int i = 0; i < particle.transform.GetComponent<ParticleData>()._ParticleLists.Count; i++)
        {
            particle.transform.GetComponent<ParticleData>()._ParticleLists[i].GetComponent<AudioSyncWithParticleSimulation>().bias = ExportManager.instance.beatValue;
            particle.transform.GetComponent<ParticleData>()._ParticleLists[i].GetComponent<AudioSyncWithParticleSimulation>().max = ExportManager.instance.simulationValue;
        }
        if (particle.transform.GetComponent<ParticleData>()._ParticleNoiseList.Count > 0)
        {
            for (int j = 0; j < particle.transform.GetComponent<ParticleData>()._ParticleNoiseList.Count; j++)
            {
                if (particle.transform.GetComponent<ParticleData>()._ParticleNoiseList[j]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._ParticleNoiseList[j].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchroniseFlare.Count > 0)
        {
            for (int k = 0; k < particle.transform.GetComponent<ParticleData>()._SynchroniseFlare.Count; k++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchroniseFlare[k]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchroniseFlare[k].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeColor.Count > 0)
        {
            for (int l = 0; l < particle.transform.GetComponent<ParticleData>()._SynchronizeColor.Count; l++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeColor[l]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeColor[l].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeImageFill.Count > 0)
        {
            for (int m = 0; m < particle.transform.GetComponent<ParticleData>()._SynchronizeImageFill.Count; m++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeImageFill[m]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeImageFill[m].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeLight.Count > 0)
        {
            for (int n = 0; n < particle.transform.GetComponent<ParticleData>()._SynchronizeLight.Count; n++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeLight[n]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeLight[n].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeScales.Count > 0)
        {
            for (int num = 0; num < particle.transform.GetComponent<ParticleData>()._SynchronizeScales.Count; num++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeScales[num]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeScales[num].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeVibration.Count > 0)
        {
            for (int num2 = 0; num2 < particle.transform.GetComponent<ParticleData>()._SynchronizeVibration.Count; num2++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeVibration[num2]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeVibration[num2].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeUiEffectToneMode.Count > 0)
        {
            for (int num3 = 0; num3 < particle.transform.GetComponent<ParticleData>()._SynchronizeUiEffectToneMode.Count; num3++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeUiEffectToneMode[num3]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeUiEffectToneMode[num3].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeGrayscaleToneMode.Count > 0)
        {
            for (int num3 = 0; num3 < particle.transform.GetComponent<ParticleData>()._SynchronizeGrayscaleToneMode.Count; num3++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeGrayscaleToneMode[num3]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeGrayscaleToneMode[num3].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeRotation.Count > 0)
        {
            for (int num4 = 0; num4 < particle.transform.GetComponent<ParticleData>()._SynchronizeRotation.Count; num4++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeRotation[num4]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeRotation[num4].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizePosition.Count > 0)
        {
            for (int num5 = 0; num5 < particle.transform.GetComponent<ParticleData>()._SynchronizePosition.Count; num5++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizePosition[num5]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizePosition[num5].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeParticleSize.Count > 0)
        {
            for (int num6 = 0; num6 < particle.transform.GetComponent<ParticleData>()._SynchronizeParticleSize.Count; num6++)
            {
                if (particle.transform.GetComponent<ParticleData>()._SynchronizeParticleSize[num6]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ParticleData>()._SynchronizeParticleSize[num6].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ParticleData>()._SynchronizeUIGradient.Count <= 0)
        {
            return;
        }
        for (int num7 = 0; num7 < particle.transform.GetComponent<ParticleData>()._SynchronizeUIGradient.Count; num7++)
        {
            if (particle.transform.GetComponent<ParticleData>()._SynchronizeUIGradient[num7]._IsAffectedToUserSetting)
            {
                particle.transform.GetComponent<ParticleData>()._SynchronizeUIGradient[num7].bias = ExportManager.instance.beatValue;
            }
        }
    }
    public List<float> ManageTransaction(int number, AudioClip audioClip)
    {
        List<float> list = new List<float>();
        float num = number;
        float length = audioClip.length;
        float item = length / num;
        list.Add(num);
        list.Add(length);
        list.Add(item);
        return list;
    }
    private IEnumerator LoadAudio(string path, ImageSetupManager setupManager)
    {
        Debug.Log("Now we are loading a audio.! path is :- " + path);
        WWW wWW = new WWW("file://" + path);
        while (!wWW.isDone)
        {
            yield return null;
        }
        try
        {
            audioClip = wWW.GetAudioClip(threeD: false);
            audioSource.clip = audioClip;
            audioSource.Play();
            CaptureController.instance.StartCapturing();
            OnVideoStopHandler.instance.isEverythingLoaded = true;
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("LoadMusic EX" + ex.Message);
        }
        ToastManager.instance.ToAndroidClipBoard(path);
        txt.text = "Audio length :-" + audioClip.length.ToString();
        try
        {
            if (setupManager == null)
            {
                Debug.Log("Found SetupManager NULL.!");
            }
            else
            {
                setupManager.SetUpAnimationData(ManageTransaction(ExportManager.instance.id.Count, audioSource.clip));
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on :- " + ex.Message);
        }

    }
    private void ApplyText(ParticleData particleData)
    {
        foreach (string datum in ExportManager.instance._myTextData.data)
        {
            if (particleData != null)
            {
                for (int i = 0; i < particleData._Texts.Length; i++)
                {
                    for (int j = 0; j < particleData._Texts[i]._Text.Length; j++)
                    {
                        particleData._Texts[i]._Text[j].text = ExportManager.instance._myTextData.data[i];
                    }
                }
            }
        }
        int num = 0;
        foreach (string datum2 in ExportManager.instance._myTextData.data)
        {
            if (particleData != null)
            {
                for (int k = 0; k < particleData._Texts.Length; k++)
                {
                    for (int l = 0; l < particleData._Texts[k]._TextMeshPro.Length; l++)
                    {
                        particleData._Texts[k]._TextMeshPro[l].text = ExportManager.instance._myTextData.data[k];
                    }
                }
            }
            num++;
        }
    }
    private void ApplyTrasncationSettings(GameObject particle)
    {
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLensFlareBright.Length > 0)
        {
            for (int i = 0; i < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLensFlareBright.Length; i++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLensFlareBright[i]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLensFlareBright[i].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithColor.Length > 0)
        {
            for (int j = 0; j < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithColor.Length; j++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithColor[j]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithColor[j].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithImageFillAmount.Length > 0)
        {
            for (int k = 0; k < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithImageFillAmount.Length; k++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithImageFillAmount[k]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithImageFillAmount[k].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLight.Length > 0)
        {
            for (int l = 0; l < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLight.Length; l++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLight[l]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithLight[l].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithGivenAxis.Length > 0)
        {
            for (int m = 0; m < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithGivenAxis.Length; m++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithGivenAxis[m]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithGivenAxis[m].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithRandomLocalPos.Length > 0)
        {
            for (int n = 0; n < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithRandomLocalPos.Length; n++)
            {
                if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithRandomLocalPos[n]._IsAffectedToUserSetting)
                {
                    particle.transform.GetComponent<ImageSetupManager>().audioSyncWithRandomLocalPos[n].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithMtone.Length <= 0)
        {
            return;
        }
        for (int num = 0; num < particle.transform.GetComponent<ImageSetupManager>().audioSyncWithMtone.Length; num++)
        {
            if (particle.transform.GetComponent<ImageSetupManager>().audioSyncWithMtone[num]._IsAffectedToUserSetting)
            {
                particle.transform.GetComponent<ImageSetupManager>().audioSyncWithMtone[num].bias = ExportManager.instance.beatValue;
            }
        }
    }
    private void ApplyThemeSettings(GameObject theme)
    {
        if (theme.transform.GetComponent<ThemeSubDetails>().effectWithLensFlareBright.Length > 0)
        {
            for (int i = 0; i < theme.transform.GetComponent<ThemeSubDetails>().effectWithLensFlareBright.Length; i++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectWithLensFlareBright[i]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectWithLensFlareBright[i].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().effectSyncWithColor.Length > 0)
        {
            for (int j = 0; j < theme.transform.GetComponent<ThemeSubDetails>().effectSyncWithColor.Length; j++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectSyncWithColor[j]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectSyncWithColor[j].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().effectWithFilleImage.Length > 0)
        {
            for (int k = 0; k < theme.transform.GetComponent<ThemeSubDetails>().effectWithFilleImage.Length; k++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectWithFilleImage[k]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectWithFilleImage[k].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().effectWithLight.Length > 0)
        {
            for (int l = 0; l < theme.transform.GetComponent<ThemeSubDetails>().effectWithLight.Length; l++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectWithLight[l]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectWithLight[l].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().effectWithGivenAxis.Length > 0)
        {
            for (int m = 0; m < theme.transform.GetComponent<ThemeSubDetails>().effectWithGivenAxis.Length; m++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectWithGivenAxis[m]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectWithGivenAxis[m].bias = ExportManager.instance.beatValue;
                }
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().effectObjectVibration.Length > 0)
        {
            for (int n = 0; n < theme.transform.GetComponent<ThemeSubDetails>().effectObjectVibration.Length; n++)
            {
                if (theme.transform.GetComponent<ThemeSubDetails>().effectObjectVibration[n]._IsAffectedToUserSetting)
                {
                    theme.transform.GetComponent<ThemeSubDetails>().effectObjectVibration[n].bias = ExportManager.instance.beatValue;
                }
            }
        }
        for (int num = 0; num < theme.transform.GetComponent<ThemeSubDetails>().effetcMTone.Length; num++)
        {
            if (theme.transform.GetComponent<ThemeSubDetails>().effetcMTone[num]._IsAffectedToUserSetting)
            {
                theme.transform.GetComponent<ThemeSubDetails>().effetcMTone[num].bias = ExportManager.instance.beatValue;
            }
        }
        if (theme.transform.GetComponent<ThemeSubDetails>().grayscaleTone.Length <= 0)
        {
            return;
        }

        for (int num = 0; num < theme.transform.GetComponent<ThemeSubDetails>().grayscaleTone.Length; num++)
        {
            if (theme.transform.GetComponent<ThemeSubDetails>().grayscaleTone[num]._IsAffectedToUserSetting)
            {
                theme.transform.GetComponent<ThemeSubDetails>().grayscaleTone[num].bias = ExportManager.instance.beatValue;
            }
        }
    }



    private IEnumerator LoadAssetBundle(AssetBundle assetBundle, string path)
    {
        AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync<GameObject>(path);
        yield return assetBundleRequest;
        GameObject original = assetBundleRequest.asset as GameObject;
        GameObject gameObject = UnityEngine.Object.Instantiate(original, new Vector3(0f, 0f, 0f), Quaternion.identity);
        if (!(gameObject != null))
        {
            yield break;
        }
        gameObject.transform.parent = parentGO.transform;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        ApplyPaticlesSettings(gameObject);
        ApplyText(gameObject.transform.GetComponent<ParticleData>());
        Camera[] camera = gameObject.transform.GetComponent<ParticleData>()._Camera;
        foreach (Camera camera2 in camera)
        {
            if (ExportManager.instance.exportResolution == 360)
            {
                camera2.targetTexture = rend360;
            }
            else if (ExportManager.instance.exportResolution == 480)
            {
                camera2.targetTexture = rend480;
            }
            else if (ExportManager.instance.exportResolution == 720)
            {
                camera2.targetTexture = rend720;
            }
        }
    }

}
