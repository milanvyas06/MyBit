using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    public GameObject selectedPrefabTheme;

    public GameObject selectedParticleTemplate;

    public GameObject selectedParticleData;

    public GameObject selectedTranscation;

    public GameObject previewSystem;

    public GameObject particleSystem;

    public GameObject themePanel;

    public GameObject transactionPanel;
    public GameObject themes;
    public GameObject transactions;
    public bool isPlaying;
    public Button replayBtn;
    public Button replayButton;

    public Text txtTitlePnlUnlock;
    public GameObject particleScrollerMask;
    public GameObject ignore2;
    public GameObject bottomPanel;
    public Slider sliderBeat;
    public Slider sliderSimulation;
    public GameObject ignore1;
    public GameObject selectedParticle;
    [HideInInspector]
    public GameObject selectedTheme;
    [HideInInspector]
    public GameObject ingore4;
    [HideInInspector]
    public GameObject selectedThemeContainer;
    public Sprite particleSelectedBorder;
    public Sprite particleUnSelectBorder;
    public Sprite themeSelected;
    public Sprite themeUnSelected;

    [HideInInspector]
    public Sprite ignore;[HideInInspector]
    public Sprite ignore3;
    [Space]
    public GameObject currentlySelectedCategory;
    [Space]
    public TextData inputedText;
    [Space]
    public GameObject noTxtPanel;
    public GameObject UnlockParticlePanel;
    public GameObject returnAssetLoading;
    public GameObject adLoadingPanel;

    public GameObject ingore;

    public Sprite[] isFavorite;

    public Image favParticle;

    public GameObject generalContent;


    public Sprite sp1;

    public Sprite sp2;

    public GameObject res360;

    public GameObject res540;

    public GameObject res720;

    public GameObject videoQuality;

    public bool isInteruppt;

    public GameObject favorite;

    public Image layerIndex;

    public bool isCategoryPanelIn;

    public Image changeLayer;

    public Text panelDetails;

    public AssetBundle inProcessAssetBundle;

    public GameObject _favorite;

    public GameObject _favorite1;

    public GameObject _category;

    public GameObject _PreviewLoadingPanel;

    public Sprite[] selectUnSelectCategory;
    public Text px112Text;
    public GameObject unlockTranscationPanel;
    public GameObject unlockThemePanel;
    public Text transcationName; public Text themeName;
    public GameObject transcationInProcessPrefab;
    public GameObject _panel;
    public GameObject replyButton;
    public GameObject pointCheck;
    public string catName;
    public GameObject transcationContent;
    [Space]
    public GameObject editTextPanel;

    public TMP_InputField editTextBox;


    public GameObject waterMarkPanel;

    public GameObject waterMarkButton;
    public GameObject adNotLoadedPanel;

    public Text previewText;

    private bool isInTransition = false;

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    static AndroidJavaObject activityContext;


    private void Awake()
    {
        instance = this;
        Screen.sleepTimeout = -1;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android) ADManager.Instance.LoadBannerAds();

        ExportManager.instance.beatValue = Mathf.Abs(sliderBeat.value);
        ExportManager.instance.simulationValue = sliderSimulation.value;

        waterMarkButton.SetActive(!(ExportManager.instance.shouldRemoveWatermark));

        if (ToastManager.instance.shouldLoadVideo)
        {
            ToastManager.instance.shouldLoadVideo = false;
            try
            {
            }
            catch (Exception ex)
            {
                ToastManager.instance.ToAndroidClipBoard("E1 : " + ex.Message);
            }
        }
        if (Application.platform == RuntimePlatform.Android)
        {

            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginClass = new AndroidJavaClass(PackageManager.PLUGINPACKAGENAME);
            _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("instance");
            _pluginInstance.CallStatic("setContext", activityContext);

            String videoDir = _pluginInstance.CallStatic<string>("getRfailed");

            if (videoDir.Equals("true"))
            {
                _pluginInstance.CallStatic("setRfailedfalse", activityContext);
            }
        }


        ExportManager.instance.exportResolution = 720;
        ToastManager.instance.ToAndroidClipBoard("Transition : " + UnlockTransaction.instance.IsUnlockedAllTransition() + "  Partical: " + UnlockParticleManager.instance.IsUnlockedAllParticle());
        if (UnlockTransaction.instance.IsUnlockedAllTransition() && UnlockParticleManager.instance.IsUnlockedAllParticle())
        {
            UnityEngine.PlayerPrefs.SetString("IsAllUnlock", "1");
        }
        else
        {
            UnityEngine.PlayerPrefs.SetString("IsAllUnlock", "0");
        }

    }

    private void Update()
    {
        if (!isInteruppt)
        {
            if (!AudioHelperMain.instance.audioSourceAndroid.isPlaying)
            {
                replayBtn.gameObject.SetActive(value: true);
                replayButton.gameObject.SetActive(value: true);
                replyButton.gameObject.SetActive(value: true);
                Time.timeScale = 0f;
                isPlaying = true;
            }
            else
            {
                replayBtn.gameObject.SetActive(value: false);
                replayButton.gameObject.SetActive(value: false);
                replyButton.gameObject.SetActive(value: false);
                Time.timeScale = 1f;
                isPlaying = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInteruppt)
            {
                isInteruppt = false;


                waterMarkPanel.GetComponent<Animator>().Play("WaterMarkPanelOut");


                Time.timeScale = 1f;
                AudioHelperMain.instance.audioSourceAndroid.Play();
            }
            else
            {
                try
                {
                    CloseADNotLoadPopup();
                    HidTransactionPanel();
                    HidThemePanel();

                    bool isActive = noTxtPanel.transform.GetChild(1).GetComponent<RectTransform>().position.x > 0 ? true : false;

                    if (isActive)
                        noTxtPanel.transform.GetComponent<Animator>().Play("OUT");
                    CategoryManager.instance.ResetEverything();

                    if (Application.platform == RuntimePlatform.Android)
                    {
                        instance._PreviewLoadingPanel.SetActive(value: true);
                        AudioHelperMain.instance.audioSourceAndroid.mute = true;
                        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                        {
                            _pluginInstance.CallStatic("StartMain", activityContext);
                        }));
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }

    public void OnDirectGotoHome()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            instance._PreviewLoadingPanel.SetActive(value: true);
            AudioHelperMain.instance.audioSourceAndroid.mute = true;
            activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.CallStatic("StartApp", activityContext);
            }));
        }
    }

    public void SynchronizeLastCategorySelection(GameObject go)
    {
        try
        {
            currentlySelectedCategory.transform.GetComponent<Image>().sprite = selectUnSelectCategory[1];
        }
        catch (Exception)
        {
        }
        currentlySelectedCategory = go;
        currentlySelectedCategory.transform.GetComponent<Image>().sprite = selectUnSelectCategory[0];
    }

    public void ApplyingBeatSettingForMultipleTheme(ImageSetupManager imgSetupManager)
    {
        for (int i = 0; i < imgSetupManager.audioSyncWithColor.Length; i++)
        {
            if (imgSetupManager.audioSyncWithColor[i]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithColor[i].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int j = 0; j < imgSetupManager.audioSyncWithImageFillAmount.Length; j++)
        {
            if (imgSetupManager.audioSyncWithImageFillAmount[j]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithImageFillAmount[j].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int k = 0; k < imgSetupManager.audioSyncWithLight.Length; k++)
        {
            if (imgSetupManager.audioSyncWithLight[k]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithLight[k].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int l = 0; l < imgSetupManager.audioSyncWithGivenAxis.Length; l++)
        {
            if (imgSetupManager.audioSyncWithGivenAxis[l]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithGivenAxis[l].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int m = 0; m < imgSetupManager.audioSyncWithRandomLocalPos.Length; m++)
        {
            if (imgSetupManager.audioSyncWithRandomLocalPos[m]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithRandomLocalPos[m].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int n = 0; n < imgSetupManager.audioSyncWithLensFlareBright.Length; n++)
        {
            if (imgSetupManager.audioSyncWithLensFlareBright[n]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithLensFlareBright[n].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num = 0; num < imgSetupManager.audioSyncWithMtone.Length; num++)
        {
            if (imgSetupManager.audioSyncWithMtone[num]._IsAffectedToUserSetting)
            {
                imgSetupManager.audioSyncWithMtone[num].bias = Mathf.Abs(sliderBeat.value);
            }
        }
    }

    public void HidThemePanel()
    {
        themePanel.SetActive(value: false);
        bottomPanel.SetActive(value: true);
        particleScrollerMask.SetActive(value: true);
    }

    public void favouriteTemplateManagement(int id)
    {
        if (PlayerPrefs.instance.IsFav(id, ExportManager.instance.shouldLoadPartileFromBundle))
        {
            PlayerPrefs.instance.RemoveFromFavouriteTemplate(id, ExportManager.instance.shouldLoadPartileFromBundle);
            favParticle.sprite = isFavorite[1];
            for (int i = 0; i < CategoryManager.instance.favoriteTemplates.Count; i++)
            {
                FavouriteTemplateDetail favouriteTemplateDetail = CategoryManager.instance.favoriteTemplates[i];
                if (favouriteTemplateDetail.UniqueNo == id)
                {
                    FavouriteTemplateDetail favouriteTemplateDetail2 = CategoryManager.instance.favoriteTemplates[i];
                    UnityEngine.Object.Destroy(favouriteTemplateDetail2.ParticleButton);
                    CategoryManager.instance.favoriteTemplates.RemoveAt(i);
                }
            }
            if (CategoryManager.instance.favoriteTemplates.Count <= 0)
            {
                favorite.SetActive(value: false);
                if (isCategoryPanelIn)
                {
                    _favorite.transform.GetComponent<Animator>().Play("GeneralOUT");
                    _category.transform.GetComponent<Animator>().Play("CategoryIN");
                    isCategoryPanelIn = false;
                }
            }
            else if (CategoryManager.instance.favoriteTemplates.Count > 0)
            {
                favorite.SetActive(value: true);
            }
        }
        else
        {
            PlayerPrefs.instance.SetToFavouriteTemplate(id, ExportManager.instance.shouldLoadPartileFromBundle);
            favParticle.sprite = isFavorite[0];
            int shortingNo = PlayerPrefs.instance.ShortingNo(id, ExportManager.instance.shouldLoadPartileFromBundle);
            if (ExportManager.instance.shouldLoadPartileFromBundle)
            {
                CategoryManager.instance.FavouriteParticleButtonGenerator(id, ExportManager.instance.particlePrefab, shortingNo, flag: true, ExportManager.instance.particlePath, ExportManager.instance.shouldLoadPartileFromBundle, ExportManager.instance.thumbPath, ExportManager.instance.prefabName, ExportManager.instance.themeName, string.Empty);
            }
            else
            {
                CategoryManager.instance.FavouriteParticleButtonGenerator(id, ExportManager.instance.particlePrefab, shortingNo, flag: true, string.Empty, ExportManager.instance.shouldLoadPartileFromBundle, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            if (CategoryManager.instance.favoriteTemplates.Count <= 0)
            {
                favorite.SetActive(value: false);
                _favorite.transform.GetComponent<Animator>().Play("GeneralOUT");
                _category.transform.GetComponent<Animator>().Play("CategoryIN");
            }
            else if (CategoryManager.instance.favoriteTemplates.Count > 0)
            {
                favorite.SetActive(value: true);
            }
        }
    }

    public void OnLayerButtonClicked()
    {
        LayerManagement.instance.OnLayerButtonClicked();
    }

    public void OnSelectQuality(int num)
    {
        switch (num)
        {
            case 360:
                res360.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp1;
                res540.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                res720.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                ExportManager.instance.exportResolution = 360;
                FinalExport();
                break;
            case 480:
                res360.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                res540.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp1;
                res720.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                ExportManager.instance.exportResolution = 480;
                FinalExport();
                break;
            case 720:
                res360.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                res540.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp2;
                res720.transform.GetChild(0).transform.GetComponent<Image>().sprite = sp1;
                ExportManager.instance.exportResolution = 720;
                FinalExport();
                break;
        }
    }

    public void FinalExport()
    {
        if (Application.platform == RuntimePlatform.Android) ADManager.Instance.DestoryBannerAds();
        isInteruppt = false;
        if (ToastManager.instance.effectName == "0")
        {
            ToastManager.instance.musicPathA = AudioHelperMain.instance.audioSourceAndroid.clip.name + ".mp3";
        }
        SceneManager.LoadScene("_ExportScene");
    }

    public void OnFavouriteThemeClicked(GameObject _Panel)
    {
        _favorite.transform.GetComponent<Animator>().Play("GeneralIN");
        _category.transform.GetComponent<Animator>().Play("CategoryOUT");
        SynchronizeLastCategorySelection(_favorite1);
        isCategoryPanelIn = true;
    }

    public void HidParticlePanel()
    {
        particleScrollerMask.SetActive(value: false);
    }

    public void OnReplayClick()
    {
        if (!ExportManager.instance.selectedMultipleImage)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
            return;
        }
        Time.timeScale = 1f;
        AudioHelperMain.instance.audioSourceAndroid.Play();
        TransactionManager.instance.RestartTransactionInPreview();
        TransactionManager.instance.RestartTransactionInPrafab();
    }

    public void HidTransactionPanel()
    {
        transactionPanel.transform.localScale = new Vector3(0f, 0f, 0f);
        bottomPanel.SetActive(value: true);
        particleScrollerMask.SetActive(value: true);
    }
    public void ShowTransactionPanel()
    {
        transactionPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        bottomPanel.SetActive(value: false);
        particleScrollerMask.SetActive(value: false);
        transcationContent = transactionPanel.transform.GetChild(0).GetChild(0).transform.gameObject;
        StartCoroutine(LoadTranscation(transcationContent));
    }

    public void OnQualityPanelOut()
    {
        isInteruppt = false;
        videoQuality.transform.GetComponent<Animator>().Play("OUT");
        Time.timeScale = 1f;
        AudioHelperMain.instance.audioSourceAndroid.Play();
    }

    public void ShowPhotosPanel()
    {
        _PreviewLoadingPanel.SetActive(value: true);
        if (!AudioHelperMain.instance.audioSourceAndroid.isPlaying)
        {
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
        AudioHelperMain.instance.audioSourceAndroid.mute = true;
        Debug.Log("Opening gallery.!");
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.CallStatic("OpenGallery", activityContext);
            }));
        }
        _PreviewLoadingPanel.SetActive(false);
    }

    public void OnFavouriteClick()
    {
        favouriteTemplateManagement(ExportManager.instance.particleToUnlock);
    }

    public void OnEditableTextWarningPanelBackClick()
    {
        noTxtPanel.transform.GetComponent<Animator>().Play("OUT");
    }

    public void RefreshSimulationSlider()
    {
        sliderSimulation.value = 8f;
    }

    public void OnCancelClick()
    {
        videoQuality.transform.GetComponent<Animator>().Play("OUT");
    }

    public void Export()
    {
        AudioHelperMain.instance.audioSourceAndroid.Pause();
        videoQuality.transform.GetComponent<Animator>().Play("IN");
    }

    public void BeatsController()
    {
        ExportManager.instance.beatValue = Mathf.Abs(sliderBeat.value);
        try
        {
            ParticleData component = selectedParticleTemplate.GetComponent<ParticleData>();
            ApplyBeatSettingForParticles(component);
        }
        catch (Exception)
        {
        }
        try
        {
            ThemeSubDetails component2 = selectedPrefabTheme.GetComponent<ThemeSubDetails>();
            ApplyingBeatSettingForTheme(component2);
        }
        catch (Exception)
        {
        }
        try
        {
            ImageSetupManager component3 = selectedTranscation.GetComponent<ImageSetupManager>();
            ApplyingBeatSettingForMultipleTheme(component3);
        }
        catch (Exception)
        {
        }
    }

    public void ShowAudioPanel()
    {
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    _pluginInstance.CallStatic("OpenSongChooser", activityContext);
                }));
            }
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("E1 : " + ex.Message);
        }
    }

    public void EnablePanel(string str)
    {
        _panel.SetActive(value: true);
    }

    public void FailedToLoadVideoAds(string flag)
    {
        if (selectedParticleTemplate == null)
        {
            ParticlePrefab particlePrefab = CategoryManager.instance.catList[0]._myParticleList._particlePrefab[0];
            LoadTemplate.instance.LoadParticleTemplate(CategoryManager.instance.catList[0]._myParticleList._particlePrefab[0].Prefab, CategoryManager.instance.catList[0]._myParticleList._ParticleButtonList[0], 18, particlePrefab);
        }
        AudioHelperMain.instance.audioSourceAndroid.transform.gameObject.SetActive(value: true);
        UnlockParticlePanel.transform.GetComponent<Animator>().Play("OUT");
        previewSystem.SetActive(value: false);
        particleSystem.SetActive(value: true);
        UnityEngine.Object.Destroy(selectedParticleData);
    }

    public void ShowThemePanel()
    {
        themePanel.SetActive(value: true);
        bottomPanel.SetActive(value: false);
        particleScrollerMask.SetActive(value: false);
    }

    public void ApplyingBeatSettingForTheme(ThemeSubDetails themeDetails)
    {
        for (int i = 0; i < themeDetails.effectSyncWithColor.Length; i++)
        {
            if (themeDetails.effectSyncWithColor[i]._IsAffectedToUserSetting)
            {
                themeDetails.effectSyncWithColor[i].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int j = 0; j < themeDetails.effectWithFilleImage.Length; j++)
        {
            if (themeDetails.effectWithFilleImage[j]._IsAffectedToUserSetting)
            {
                themeDetails.effectWithFilleImage[j].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int k = 0; k < themeDetails.effectWithLight.Length; k++)
        {
            if (themeDetails.effectWithLight[k]._IsAffectedToUserSetting)
            {
                themeDetails.effectWithLight[k].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int l = 0; l < themeDetails.effectWithGivenAxis.Length; l++)
        {
            if (themeDetails.effectWithGivenAxis[l]._IsAffectedToUserSetting)
            {
                themeDetails.effectWithGivenAxis[l].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int m = 0; m < themeDetails.effectObjectVibration.Length; m++)
        {
            if (themeDetails.effectObjectVibration[m]._IsAffectedToUserSetting)
            {
                themeDetails.effectObjectVibration[m].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int n = 0; n < themeDetails.effectWithLensFlareBright.Length; n++)
        {
            if (themeDetails.effectWithLensFlareBright[n]._IsAffectedToUserSetting)
            {
                themeDetails.effectWithLensFlareBright[n].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num = 0; num < themeDetails.effetcMTone.Length; num++)
        {
            if (themeDetails.effetcMTone[num]._IsAffectedToUserSetting)
            {
                themeDetails.effetcMTone[num].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num = 0; num < themeDetails.grayscaleTone.Length; num++)
        {
            if (themeDetails.grayscaleTone[num]._IsAffectedToUserSetting)
            {
                themeDetails.grayscaleTone[num].bias = Mathf.Abs(sliderBeat.value);
            }
        }
    }

    public void HidTextPanel()
    {
        ignore2.transform.localScale = new Vector3(0f, 0f, 0f);
    }


    public void ShowTextPanel()
    {
        px112Text.text = ExportManager.instance._myTextData.data.Count.ToString();
        if (ExportManager.instance._myTextData.data.Count < 1)
        {
            noTxtPanel.transform.GetComponent<Animator>().Play("IN");
            if (UnityEngine.PlayerPrefs.HasKey("LastEditedText"))
            {
                string lastEditedText = UnityEngine.PlayerPrefs.GetString("LastEditedText");
                TextData textData = new TextData();
                textData.data = new System.Collections.Generic.List<string>();
                textData.data.Clear();
                textData.data.Add(lastEditedText);
                ExportManager.instance._myTextData = textData;
                EditTextHelper.instance.LoadText(textData.data);
            }
            return;
        }
        string text = JsonUtility.ToJson(ExportManager.instance._myTextData);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    _pluginInstance.CallStatic("OpenEditText", activityContext);
                }));
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Show Text panel throws error.! " + ex.Message);
        }
    }


    public void OnHideEditTextPanel()
    {
        editTextPanel.GetComponent<Animator>().Play("EditTextOut");
    }

    public void ApplyBeatSettingForParticles(ParticleData particleData)
    {
        for (int i = 0; i < particleData._ParticleLists.Count; i++)
        {
            if (particleData._ParticleLists[i]._IsAffectedToUserSetting)
            {
                particleData._ParticleLists[i].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int j = 0; j < particleData._ParticleNoiseList.Count; j++)
        {
            if (particleData._ParticleNoiseList[j]._IsAffectedToUserSetting)
            {
                particleData._ParticleNoiseList[j].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int k = 0; k < particleData._SynchronizeColor.Count; k++)
        {
            if (particleData._SynchronizeColor[k]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeColor[k].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int l = 0; l < particleData._SynchronizeImageFill.Count; l++)
        {
            if (particleData._SynchronizeImageFill[l]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeImageFill[l].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int m = 0; m < particleData._SynchronizeLight.Count; m++)
        {
            if (particleData._SynchronizeLight[m]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeLight[m].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int n = 0; n < particleData._SynchronizeScales.Count; n++)
        {
            if (particleData._SynchronizeScales[n]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeScales[n].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num = 0; num < particleData._SynchronizeVibration.Count; num++)
        {
            if (particleData._SynchronizeVibration[num]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeVibration[num].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num2 = 0; num2 < particleData._SynchronizeUiEffectToneMode.Count; num2++)
        {
            if (particleData._SynchronizeUiEffectToneMode[num2]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeUiEffectToneMode[num2].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num2 = 0; num2 < particleData._SynchronizeGrayscaleToneMode.Count; num2++)
        {
            if (particleData._SynchronizeGrayscaleToneMode[num2]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeGrayscaleToneMode[num2].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num3 = 0; num3 < particleData._SynchroniseFlare.Count; num3++)
        {
            if (particleData._SynchroniseFlare[num3]._IsAffectedToUserSetting)
            {
                particleData._SynchroniseFlare[num3].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num4 = 0; num4 < particleData._SynchronizeRotation.Count; num4++)
        {
            if (particleData._SynchronizeRotation[num4]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeRotation[num4].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num5 = 0; num5 < particleData._SynchronizePosition.Count; num5++)
        {
            if (particleData._SynchronizePosition[num5]._IsAffectedToUserSetting)
            {
                particleData._SynchronizePosition[num5].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num6 = 0; num6 < particleData._SynchronizeParticleSize.Count; num6++)
        {
            if (particleData._SynchronizeParticleSize[num6]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeParticleSize[num6].bias = Mathf.Abs(sliderBeat.value);
            }
        }
        for (int num7 = 0; num7 < particleData._SynchronizeUIGradient.Count; num7++)
        {
            if (particleData._SynchronizeUIGradient[num7]._IsAffectedToUserSetting)
            {
                particleData._SynchronizeUIGradient[num7].bias = Mathf.Abs(sliderBeat.value);
            }
        }
    }

    public void ShowParticlePanel()
    {
        particleScrollerMask.SetActive(value: true);
    }

    public void SimulationController()
    {
        ExportManager.instance.simulationValue = sliderSimulation.value;
        ParticleData component = selectedParticleTemplate.GetComponent<ParticleData>();
        for (int i = 0; i < component._ParticleLists.Count; i++)
        {
            if (component._ParticleLists[i]._IsAffectedToUserSetting)
            {
                component._ParticleLists[i].max = Mathf.Abs(sliderSimulation.value);
            }
        }
        for (int j = 0; j < component._ParticleNoiseList.Count; j++)
        {
            if (component._ParticleNoiseList[j]._IsAffectedToUserSetting)
            {
                component._ParticleNoiseList[j].bias = Mathf.Abs(sliderSimulation.value);
                component._ParticleNoiseList[j]._beatNoice = new Vector3(Mathf.Abs(sliderSimulation.value), Mathf.Abs(sliderSimulation.value), Mathf.Abs(sliderSimulation.value));
            }
        }
    }

    private IEnumerator LoadTranscation(GameObject go)
    {
        yield return new WaitForSeconds(5E-05f);
        Transform transform = go.transform;
        Vector3 localPosition = go.transform.localPosition;
        float y = localPosition.y;
        Vector3 localPosition2 = go.transform.localPosition;
        transform.localPosition = new Vector3(0f, y, localPosition2.z);
    }

    public void RefreshBeatsSlider()
    {
        sliderBeat.value = 14f;
    }

    public void OnFavouriteThemeBackClicked(GameObject _Panel)
    {
        _favorite.transform.GetComponent<Animator>().Play("GeneralOUT");
        _category.transform.GetComponent<Animator>().Play("CategoryIN");
        isCategoryPanelIn = false;
    }

    public void OnClickDirectExportVideo()
    {
        ExportManager.instance.exportResolution = 720;
        FinalExport();
    }








    private IEnumerator ShowRewardAd()
    {
        adLoadingPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (!ADManager.Instance.ShowRewardAds())
        {
            adNotLoadedPanel.SetActive(true);
            ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
        }
        adLoadingPanel.SetActive(false);
    }



    #region OnUnLockPanelOpen
    public void OnRemoveWaterMarkClick()
    {
        isInteruppt = true;
        waterMarkPanel.GetComponent<Animator>().Play("WatermarkpanelIn");
    }
    #endregion

    #region OnWatchVideoClick

    public void OnUnlockTransactionClicked()
    {
        adLoadingPanel.SetActive(true);

        int particleId = ExportManager.instance.themeToUnlock;
        unlockTranscationPanel.transform.GetComponent<Animator>().Play("OUT");
        TransactionManager.instance.theme.transform.localScale = new Vector3(1f, 1f, 1f);
        TransactionManager.instance.previewTheme.transform.localScale = new Vector3(0f, 0f, 0f);
        UnityEngine.Object.Destroy(transcationInProcessPrefab);
        try
        {
            ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect += ADManager.Instance.RewardToUserByUnlockAnEffect;
            ADManager.Instance.googleHanlder.onRewardAdFailedToLoad += OnRewardAdFailedToLoad;
            ADManager.Instance.googleHanlder.onRewardAdLoaded += OnRewardAdLoaded;

            ADManager.Instance.LoadRewardAd();

            Debug.Log("Partical Id : " + particleId + ":1");
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("VideoAds Error : " + ex.Message);
        }
    }

    public void OnUnlockThemeClicked()
    {
        adLoadingPanel.SetActive(true);

        int particleId = ExportManager.instance.themeToUnlock;
        Debug.Log("ExportController.instance.themeToUnlock is + " + particleId);
        unlockThemePanel.transform.GetComponent<Animator>().Play("OUT");
        ThemeManager.instance.themes.transform.localScale = Vector3.one;

        UnityEngine.Object.Destroy(transcationInProcessPrefab);
        try
        {
            ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect += ADManager.Instance.RewardToUserByUnlockAnEffect;
            ADManager.Instance.googleHanlder.onRewardAdFailedToLoad += OnRewardAdFailedToLoad;
            ADManager.Instance.googleHanlder.onRewardAdLoaded += OnRewardAdLoaded;

            ADManager.Instance.LoadRewardAd();

            Debug.Log("Partical Id : " + particleId + ":1");
        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on OnUnlockThemeClicked " + ex.Message);
            ToastManager.instance.ToAndroidClipBoard("VideoAds Error : " + ex.Message);
        }
    }

    public void OnWatchVideoClick()
    {
        isInteruppt = false;
        adLoadingPanel.SetActive(true);

        waterMarkPanel.GetComponent<Animator>().Play("WaterMarkPanelOut");
        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark += ADManager.Instance.RemoveWaterMark;
        ADManager.Instance.googleHanlder.onRewardAdLoaded += OnRewardAdLoaded;
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad += OnRewardAdFailedToLoad;
        ADManager.Instance.LoadRewardAd();
    }

    public void OnUnlockClicked()
    {
        int idToUnlock = ExportManager.instance.particleToUnlock;
        UnlockParticlePanel.transform.GetComponent<Animator>().Play("OUT");
        try
        {
            Debug.Log("Partical Id : " + idToUnlock + ":0");
        }
        catch (Exception ex)
        {
            ToastManager.instance.ToAndroidClipBoard("VideoAds Error : " + ex.Message);
        }
        previewSystem.SetActive(value: false);
        particleSystem.SetActive(value: true);
        UnityEngine.Object.Destroy(selectedParticleData);
    }
    #endregion

    #region OnCancelClick

    public void OnUnlockTransactionCancelClicked()
    {
        unlockTranscationPanel.transform.GetComponent<Animator>().Play("OUT");
        TransactionManager.instance.theme.transform.localScale = new Vector3(1f, 1f, 1f);
        TransactionManager.instance.previewTheme.transform.localScale = new Vector3(0f, 0f, 0f);
        UnityEngine.Object.Destroy(instance.transcationInProcessPrefab);
        AudioHelperMain.instance.audioSourceAndroid.Play();
        TransactionManager.instance.RestartTransactionInPreview();
        TransactionManager.instance.RestartTransactionInPrafab();

        ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect -= ADManager.Instance.RewardToUserByUnlockAnEffect;
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdFailedToLoad;
        ADManager.Instance.googleHanlder.onRewardAdLoaded -= OnRewardAdLoaded;
    }

    public void OnUnlockThemeCancelClicked()
    {
        unlockThemePanel.transform.GetComponent<Animator>().Play("OUT");
        ThemeManager.instance.themes.transform.localScale = Vector3.one;
        UnityEngine.Object.Destroy(instance.transcationInProcessPrefab);
        AudioHelperMain.instance.audioSourceAndroid.Play();

        ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect -= ADManager.Instance.RewardToUserByUnlockAnEffect;
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdFailedToLoad;
        ADManager.Instance.googleHanlder.onRewardAdLoaded -= OnRewardAdLoaded;
    }

    public void OnCancelWatermarkClick()
    {
        isInteruppt = false;
        waterMarkPanel.GetComponent<Animator>().Play("WaterMarkPanelOut");

        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
        ADManager.Instance.googleHanlder.onRewardAdLoaded -= OnRewardAdLoaded;
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdFailedToLoad;
    }

    public void OnUnlockCancelClicked()
    {
        Debug.Log(" Unlock Button Clicked.!");
        if (selectedParticleTemplate == null)
        {
            ParticlePrefab particlePrefab = CategoryManager.instance.catList[0]._myParticleList._particlePrefab[0];
            LoadTemplate.instance.LoadParticleTemplate(CategoryManager.instance.catList[0]._myParticleList._particlePrefab[0].Prefab, CategoryManager.instance.catList[0]._myParticleList._ParticleButtonList[0], 18, particlePrefab);
        }
        AudioHelperMain.instance.audioSourceAndroid.transform.gameObject.SetActive(value: true);
        UnlockParticlePanel.transform.GetComponent<Animator>().Play("OUT");
        previewSystem.SetActive(value: false);
        particleSystem.SetActive(value: true);
        UnityEngine.Object.Destroy(selectedParticleData);
    }


    #endregion

    #region REwARD AD EVENTS
    private void OnRewardAdFailedToLoad()
    {
        Debug.Log("OnRewardAdFailedToLoad " + ADManager.Instance.rewardADState);
        adLoadingPanel.SetActive(false);
        adNotLoadedPanel.SetActive(true);

        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdFailedToLoad;
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdLoaded;
        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
        ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect -= ADManager.Instance.RewardToUserByUnlockAnEffect;
    }

    private void OnRewardAdLoaded()
    {
        adLoadingPanel.SetActive(false);
        ADManager.Instance.ShowRewardAds();
    }
    public void CloseADNotLoadPopup()
    {
        adNotLoadedPanel.SetActive(false);
    }
    public void CancelAd()
    {
        adLoadingPanel.SetActive(false);
        ADManager.Instance.googleHanlder.onRewardAdFailedToLoad -= OnRewardAdFailedToLoad;
        ADManager.Instance.googleHanlder.onRewardAdLoaded -= OnRewardAdLoaded;
        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
        ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect -= ADManager.Instance.RewardToUserByUnlockAnEffect;
    }
    #endregion
}

