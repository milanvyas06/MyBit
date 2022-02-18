using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    public struct IsBundleisRegisterd
    {
        public bool isUIdMatched;

        public string bundleName;
    }

    public static CategoryManager instance;

    [SerializeField]
    private bool shouldLoadSingle;

    private bool isCategoryGenerated;

    [HideInInspector]
    public GameObject particle;

    [HideInInspector]
    public GameObject particleButton;

    public GameObject audio;

    public GameObject selectedCategoryBtn;

    public CategoryList[] catList;

    [Space]
    public GameObject categoryButton;

    public GameObject content;

    public GameObject categoryContainer;

    public GameObject particleScrollerMask;

    public GameObject category;

    [Space]
    public Sprite icon;

    public Sprite tempImg;

    [Space]
    public GameObject particle1;

    public GameObject generalContent;

    public List<FavouriteTemplateDetail> favoriteTemplates;

    public int[] lstOfId;

    public GameObject createMoreButton;

    [HideInInspector]
    public List<CategoryRegister> catRegisterd;

    public List<NewCategorySpriteRegister> newCategorySpriteRegister;

    public Sprite catIcon;

    [HideInInspector]
    public WebParticalJsonData.WaitingForDecryptedData dataDecrypter;

    [HideInInspector]
    public List<GameObject> lstOfGeneratedContainer = new List<GameObject>();


    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    static AndroidJavaObject activityContext;

    private void Awake()
    {
        instance = this;
        CategoryList[] cateGoryList = catList;
        foreach (CategoryList singleCategory in cateGoryList)
        {
            GameObject categortyButton = UnityEngine.Object.Instantiate(categoryButton, new Vector3(0f, 0f, 0f), Quaternion.identity);
            GameObject generatedCategoryContainer = UnityEngine.Object.Instantiate(categoryContainer, new Vector3(0f, 0f, 0f), Quaternion.identity);
            categortyButton.transform.parent = content.transform;
            generatedCategoryContainer.transform.parent = particleScrollerMask.transform;
            categortyButton.transform.localScale = new Vector3(1f, 1f, 1f);
            generatedCategoryContainer.transform.localScale = new Vector3(1f, 1f, 1f);
            generatedCategoryContainer.transform.name = singleCategory.CategoryName + "_Container";
            RectTransform component = generatedCategoryContainer.transform.GetComponent<RectTransform>();
            component.offsetMin = new Vector2(-720f, 0f);
            component.offsetMax = new Vector2(720f, 142f);
            generatedCategoryContainer.transform.localPosition = new Vector3(0f, 0f, 0f);
            singleCategory._myParticleList.ParticleButtonParent = generatedCategoryContainer.transform.GetComponent<CategoryContainer>().content;
            singleCategory._myParticleList.GenerateParticleList(singleCategory.CategoryName);
            categortyButton.transform.GetComponent<CategoryButtonDetail>().name = singleCategory.CategoryName;
            categortyButton.transform.GetComponent<CategoryButtonDetail>().catName = singleCategory.CategoryName;
            categortyButton.transform.GetComponent<CategoryButtonDetail>().catIcon.sprite = singleCategory.CatIcon;
            categortyButton.transform.GetChild(0).transform.GetComponent<Text>().text = singleCategory.CategoryName;
            categortyButton.transform.GetComponent<Image>().sprite = SettingManager.instance.selectUnSelectCategory[1];
            singleCategory._myParticleList._Button = categortyButton;
            CreateMoreButton(singleCategory._myParticleList.ParticleButtonParent, singleCategory.CategoryName, isGeneratedJson: false, generatedCategoryContainer, categortyButton, singleCategory);

            lstOfGeneratedContainer.Add(generatedCategoryContainer);

            categortyButton.transform.GetComponent<Button>().onClick.AddListener(delegate
            {
                ClickOnCategory(singleCategory.CategoryName, generatedCategoryContainer, categortyButton);
            });
            generatedCategoryContainer.transform.GetComponent<CategoryContainer>().backButton.onClick.AddListener(delegate
            {
                ClickOnBackFromItemList(generatedCategoryContainer);
            });
            generatedCategoryContainer.GetComponent<CategoryContainer>().animator.Play("GeneralOUT");
        }
        FirstTimeCategoryIn();
        isCategoryGenerated = true;
    }

    public void ResetEverything()
    {
        foreach (var item in lstOfGeneratedContainer)
        {
            item.GetComponent<CategoryContainer>().animator.Play("GeneralOUT");
        }
        FirstTimeCategoryIn();
    }

    public void FirstTimeCategoryIn()
    {
        category.transform.GetComponent<Animator>().Play("CategoryIN");
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginClass = new AndroidJavaClass(PackageManager.PLUGINPACKAGENAME);
            _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("instance");
            _pluginInstance.CallStatic("setContext", activityContext);
        }

        StartCoroutine(FavoriteManager());
        if (Application.platform != RuntimePlatform.Android)
        {
            string imgPath = "E:/Free Pic/XYZ/1.png?E:/Free Pic/XYZ/2.png?E:/Free Pic/XYZ/3.png?E:/Free Pic/XYZ/4.png?E:/Free Pic/XYZ/5.png?E:/Free Pic/XYZ/6.png";
            if (shouldLoadSingle)
            {
                var data = imgPath.Split('?');
                LoadPhotoInEditor(data[0]);
            }
            else
            {

                LoadDefaultMultipleImageData(imgPath);
            }
        }
    }

    public void OpenCategory(string CategoryName)
    {
        bool flag = false;
        for (int i = 0; i < catRegisterd.Count; i++)
        {
            if (catRegisterd[i].catName == CategoryName)
            {
                if (catRegisterd[i].isGeneratedFromJson)
                {
                    string catName = catRegisterd[i].catName;
                    GameObject categoryContainer = catRegisterd[i].CategoryContainer;
                    GameObject generatedcategoryButton = catRegisterd[i].GeneratedcategoryButton;
                    ClickOnCategory(catName, categoryContainer, generatedcategoryButton);
                }
                else
                {
                    string catName2 = catRegisterd[i].catName;
                    GameObject categoryContainer2 = catRegisterd[i].CategoryContainer;
                    GameObject generatedcategoryButton2 = catRegisterd[i].GeneratedcategoryButton;
                    ClickOnCategory(catName2, categoryContainer2, generatedcategoryButton2);
                }
                flag = true;
            }
        }
        if (!flag)
        {
            string catName3 = catRegisterd[0].catName;
            GameObject categoryContainer3 = catRegisterd[0].CategoryContainer;
            GameObject generatedcategoryButton3 = catRegisterd[0].GeneratedcategoryButton;
            ClickOnCategory(catName3, categoryContainer3, generatedcategoryButton3);
        }
    }

    public void LoadDefaultData(string _imgPath)
    {
        ToastManager.instance.effectName = "0";
        ExportManager.instance.audioPath = string.Empty;
        ExportManager.instance.selectionAudioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioSourceAndroid.clip = AudioHelperMain.instance.audioClip;
        AudioHelperMain.instance.audioSourceAndroid.Play();
        AudioHelperMain.instance.audioSourceAndroid.mute = false;
        FindAndLoadTemplate(18, shouldNotLoadRandomTranscation: false);
        LoadPhoto(_imgPath);
    }

    public void FavouriteParticleButtonGenerator(int uniqueNo, ParticlePrefab particlePrefab, int shortingNo, bool flag, string bundlePath, bool isBundleData, string thumbPath, string prefabName, string particleName, string downloadPath)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(particle1, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = generalContent.transform;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        if (flag)
        {
            if (isBundleData)
            {
                FavouriteTemplateDetail item = default(FavouriteTemplateDetail);
                item.ShortingNo = shortingNo;
                item.UniqueNo = uniqueNo;
                item.ParticleButton = gameObject;
                item._particlePrefab = null;
                item._bundlePath = bundlePath;
                item._thumbnailPath = thumbPath;
                item._PrefabName = prefabName;
                item.IsBundle = true;
                favoriteTemplates.Add(item);
            }
            else
            {
                FavouriteTemplateDetail item2 = default(FavouriteTemplateDetail);
                item2.ShortingNo = shortingNo;
                item2.UniqueNo = uniqueNo;
                item2.ParticleButton = gameObject;
                item2._particlePrefab = particlePrefab;
                item2._bundlePath = string.Empty;
                item2.IsBundle = false;
                favoriteTemplates.Add(item2);
            }
        }
        else
        {
            for (int i = 0; i < favoriteTemplates.Count; i++)
            {
                FavouriteTemplateDetail favouriteTemplateDetail = favoriteTemplates[i];
                if (favouriteTemplateDetail.ShortingNo == shortingNo)
                {
                    if (isBundleData)
                    {
                        FavouriteTemplateDetail value = default(FavouriteTemplateDetail);
                        value.ShortingNo = shortingNo;
                        value.UniqueNo = uniqueNo;
                        value.ParticleButton = gameObject;
                        value._particlePrefab = null;
                        value._bundlePath = bundlePath;
                        value._thumbnailPath = thumbPath;
                        value._PrefabName = prefabName;
                        value.IsBundle = isBundleData;
                        favoriteTemplates[i] = value;
                    }
                    else
                    {
                        FavouriteTemplateDetail value2 = default(FavouriteTemplateDetail);
                        value2.ShortingNo = shortingNo;
                        value2.UniqueNo = uniqueNo;
                        value2.ParticleButton = gameObject;
                        value2._particlePrefab = particlePrefab;
                        value2._bundlePath = string.Empty;
                        value2.IsBundle = isBundleData;
                        favoriteTemplates[i] = value2;
                    }
                }
            }
        }
        if (!isBundleData)
        {
            gameObject.transform.GetComponent<ParticleDataCollector>().Thumbnail.sprite = particlePrefab._Thumbnail;
            gameObject.transform.GetComponent<ParticleDataCollector>().ParticleName.text = particlePrefab._Name;
            gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
            gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.onClick.AddListener(delegate
            {
                LoadParticle(particlePrefab.Prefab, gameObject, uniqueNo, particlePrefab, flag: false, string.Empty, string.Empty, string.Empty, string.Empty, downloadPath);
            });
        }
        else
        {
            StartCoroutine(LoadTexture(thumbPath, gameObject.transform.GetComponent<ParticleDataCollector>().Thumbnail));
            gameObject.transform.GetComponent<ParticleDataCollector>().ParticleName.text = particleName;
            gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
            gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.onClick.AddListener(delegate
            {
                LoadParticle(null, gameObject, uniqueNo, particlePrefab, flag: true, bundlePath, prefabName, thumbPath, particleName, downloadPath);
            });
        }
    }


    public void LoadAssetBundle(string bundlePath)
    {
        LoadTemplate.instance.LoadParticleAssetBundle(dataDecrypter.UniqueNo, bundlePath, dataDecrypter.ThumbnailPath, dataDecrypter.ThemeName, dataDecrypter.PrefabName, dataDecrypter.ParticleButton);
    }

    public void LoadParticle(GameObject _prefab, GameObject go, int uid, ParticlePrefab particlePrefab, bool flag, string particleDataPath, string prefabName, string thumbPath, string themeName, string downloadPath)
    {
        if (!flag)
        {
            ExportManager.instance.particleToUnlock = uid;
            ExportManager.instance.particlePrefab = particlePrefab;
            ExportManager.instance.shouldLoadPartileFromBundle = false;
            if (PlayerPrefs.instance.IsFav(uid, ExportManager.instance.shouldLoadPartileFromBundle))
            {
                SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
            }
            else
            {
                SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
            }
            LoadTemplate.instance.LoadParticleTemplate(_prefab, go, uid, particlePrefab);
            return;
        }
        LoadTemplate.instance.changeBorderSprite(go);
        if (ExportManager.instance.particleToUnlock == uid)
        {
            LoadTemplate.instance.changeBorderSprite(go);
            return;
        }
        ExportManager.instance.particleToUnlock = uid;
        ExportManager.instance.shouldLoadPartileFromBundle = true;
        ExportManager.instance.thumbPath = thumbPath;
        ExportManager.instance.particlePath = particleDataPath;
        ExportManager.instance.prefabName = prefabName;
        ExportManager.instance.themeName = themeName;
        if (PlayerPrefs.instance.IsFav(uid, ExportManager.instance.shouldLoadPartileFromBundle))
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
        }
        else
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
        }
        IsBundleisRegisterd bundleRegistration = default(IsBundleisRegisterd);
        bundleRegistration = CheckIsRegisterdBundle(uid.ToString());
        if (bundleRegistration.isUIdMatched)
        {
            LoadTemplate.instance.LoadParticleAssetBundle(uid.ToString(), bundleRegistration.bundleName, string.Empty, string.Empty, prefabName, go);
            return;
        }
        dataDecrypter.UniqueNo = uid.ToString();
        dataDecrypter.ThumbnailPath = thumbPath;
        dataDecrypter.ThemeName = themeName;
        dataDecrypter.PrefabName = prefabName;
        dataDecrypter.ParticleButton = particleButton;
        dataDecrypter.DownloadAt = downloadPath;
        try
        {
            string text = particleDataPath + "?1";
            Debug.Log("Send Bundle Path With Tag 1 : " + text);
            try
            {
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.root.bridge.AndroidPluginClass");
                androidJavaClass2.CallStatic("getPrtclPth", @static, text);
            }
            catch (Exception)
            {
            }
        }
        catch (Exception)
        {
        }
    }


    public void CreateMoreButton(GameObject instantiateParent, string catName, bool isGeneratedJson, GameObject catContainer, GameObject generatedCatButton, CategoryList categoryList = null, CategoryRegister.JSonCategoryInformation jsonCatInfo = null)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(createMoreButton, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = instantiateParent.transform;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        CategoryRegister categoryRegister = new CategoryRegister();
        categoryRegister.isGeneratedFromJson = isGeneratedJson;
        categoryRegister.catName = catName;
        categoryRegister.CategoryContainer = catContainer;
        categoryRegister.GeneratedcategoryButton = generatedCatButton;
        if (jsonCatInfo != null && isGeneratedJson)
        {
            categoryRegister._JsonCategoryInformation = jsonCatInfo;
        }
        if (categoryList != null && !isGeneratedJson)
        {
            categoryRegister._CatList = categoryList;
        }
        catRegisterd.Add(categoryRegister);
        gameObject.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            OnClickToMoreButton(catName);
        });
    }

    public void LoadSound(string musicPath)
    {
        AudioHelperMain.instance.OnSelectMusic(musicPath);
    }


    public void OnClickToMoreButton(string openParticleStore)
    {
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    _pluginInstance.CallStatic("OpenParticleLibrary", activityContext, openParticleStore);
                }));
            }
            else
            {
                Debug.Log("Opening particle store in android :- " + openParticleStore);
            }
        }
        catch (Exception ex)
        {


            Debug.Log("Error Launch in OnClickToMoreButton " + ex.Message);
        }
    }

    public void LoadPhoto(string path)
    {
        ImageLoader.instance.GetSelectedImagePath(path);
    }

    public void ClickOnBackFromItemList(GameObject panel)
    {
        SettingManager.instance.catName = string.Empty;
        panel.transform.GetComponent<Animator>().Play("GeneralOUT");
        category.transform.GetComponent<Animator>().Play("CategoryIN");
    }

    public void ClickOnCategory(string catName, GameObject catContainer, GameObject generatedButton)
    {
        SettingManager.instance.catName = catName;
        catContainer.transform.GetComponent<Animator>().Play("GeneralIN");
        category.transform.GetComponent<Animator>().Play("CategoryOUT");
        SettingManager.instance.SynchronizeLastCategorySelection(generatedButton);
    }

    public void LoadDefaultMultipleImageData(string imagesPath)
    {
        ToastManager.instance.effectName = "0";
        ExportManager.instance.audioPath = string.Empty;
        ExportManager.instance.selectionAudioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioSourceAndroid.clip = AudioHelperMain.instance.audioClip;
        AudioHelperMain.instance.audioSourceAndroid.Play();
        FindAndLoadTemplate(18, shouldNotLoadRandomTranscation: true);
        ImageLoader.instance.OnSelectMultipleImg(imagesPath);
    }

    public void LoadPhotoInEditor(string path)
    {
        ToastManager.instance.effectName = "0";
        ExportManager.instance.audioPath = string.Empty;
        ExportManager.instance.selectionAudioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioClip = AudioHelperMain.instance.audioClip1;
        AudioHelperMain.instance.audioSourceAndroid.clip = AudioHelperMain.instance.audioClip;
        AudioHelperMain.instance.audioSourceAndroid.Play();
        FindAndLoadTemplate(11, shouldNotLoadRandomTranscation: true);
        ImageLoader.instance.GetSelectedImagePath(path);
    }

    private IEnumerator LoadTexture(string path, Image img)
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
            Debug.Log(ex.Message);
        }
        yield return new WaitForSeconds(0.1f);
        try
        {
            texture2D.LoadImage(data);
            Sprite sprite2 = img.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
        }
        catch (Exception ex2)
        {
            Debug.Log(ex2.Message);
        }
    }


    [Serializable]
    public class Theme
    {
        public int themeid;
        public string sound;
        public string image;
        public string songname;
        public string selectedThemeId;
    }



    public void OnLoadPreviewSceneData(string _userData)
    {

        try
        {
            Theme userapp = JsonUtility.FromJson<Theme>(_userData);

            ExportManager.instance.songName = userapp.songname;
            LoadSound(userapp.sound);
            LoadPhoto(userapp.image);
            FindAndLoadTemplate(userapp.themeid, shouldNotLoadRandomTranscation: false, Convert.ToInt16(userapp.selectedThemeId));
        }
        catch (Exception ex)
        {
            Debug.Log("Found an error on  OnLoadUserData() " + ex.Message);
        }
    }

    private IsBundleisRegisterd CheckIsRegisterdBundle(string uid)
    {
        IsBundleisRegisterd result = default(IsBundleisRegisterd);
        result.isUIdMatched = false;
        for (int i = 0; i < WebParticalJsonData.instance.particleJsonData.ParticalDetails.Count; i++)
        {
            for (int j = 0; j < WebParticalJsonData.instance.particleJsonData.ParticalDetails[i].ParticalInfo.Count; j++)
            {
                if (uid == WebParticalJsonData.instance.particleJsonData.ParticalDetails[i].ParticalInfo[j].UniqueIDNo && WebParticalJsonData.instance.particleJsonData.ParticalDetails[i].ParticalInfo[j].DecryptedBundlePath != null)
                {
                    result.isUIdMatched = true;
                    result.bundleName = WebParticalJsonData.instance.particleJsonData.ParticalDetails[i].ParticalInfo[j].DecryptedBundlePath;
                }
            }
        }
        return result;
    }

    private IEnumerator FavoriteManager()
    {
        yield return new WaitForSeconds(1f);
        isCategoryGenerated = true;
        if (favoriteTemplates.Count <= 0)
        {
            SettingManager.instance.favorite.SetActive(value: false);
        }
        else
        {
            SettingManager.instance.favorite.SetActive(value: true);
        }
        List<int> list = new List<int>();
        for (int i = 0; i < favoriteTemplates.Count; i++)
        {
            FavouriteTemplateDetail favouriteTemplateDetail = favoriteTemplates[i];
            list.Add(favouriteTemplateDetail.ShortingNo);
        }
        lstOfId = list.ToArray();
        Array.Sort(lstOfId);
        for (int j = 0; j < lstOfId.Length; j++)
        {
            try
            {
                foreach (FavouriteTemplateDetail item in favoriteTemplates)
                {
                    if (lstOfId[j] == item.ShortingNo)
                    {
                        FavouriteParticleButtonGenerator(item.UniqueNo, item._particlePrefab, item.ShortingNo, flag: false, item._bundlePath, item.IsBundle, item._thumbnailPath, item._PrefabName, item._TemplateName, item.DownloadAt);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Found an error on IEnumerator FavoriteManager() " + ex.Message);
            }

        }
    }

    public void FindAndLoadTemplate(int id, bool shouldNotLoadRandomTranscation, int themeIdNumber = -1)
    {
        for (int i = 0; i < catList.Length; i++)
        {
            for (int j = 0; j < catList[i]._myParticleList._particlePrefab.Length; j++)
            {
                if (id != catList[i]._myParticleList._particlePrefab[j].UniqueID)
                {
                    continue;
                }
                if (PlayerPrefs.instance.CheckParticleIsUnlock(catList[i]._myParticleList._particlePrefab[j].UniqueID))
                {
                    particle = catList[i]._myParticleList._particlePrefab[j].Prefab;
                    particleButton = catList[i]._myParticleList._ParticleButtonList[j];
                    ExportManager.instance.particleToUnlock = id;
                    ParticlePrefab particlePrefab = new ParticlePrefab();
                    particlePrefab = catList[i]._myParticleList._particlePrefab[j];
                    ExportManager.instance.particlePrefab = particlePrefab;
                    if (PlayerPrefs.instance.IsFav(id, shouldLoadPartileFromBundle: false))
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
                    }
                    else
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
                    }
                    LoadTemplate.instance.LoadParticleTemplate(particle, particleButton, id, particlePrefab);
                    isCategoryGenerated = false;
                }
                else if (DoNotDestroyee.instance.isAllUnlockDefault || catList[i]._myParticleList._particlePrefab[j]._UnlockDefault)
                {
                    particle = catList[i]._myParticleList._particlePrefab[j].Prefab;
                    particleButton = catList[i]._myParticleList._ParticleButtonList[j];
                    SettingManager.instance.previewSystem.SetActive(value: false);
                    SettingManager.instance.particleSystem.SetActive(value: true);
                    ExportManager.instance.particleToUnlock = id;
                    ParticlePrefab particlePrefab2 = new ParticlePrefab();
                    particlePrefab2 = catList[i]._myParticleList._particlePrefab[j];
                    ExportManager.instance.particlePrefab = particlePrefab2;
                    LoadTemplate.instance.LoadParticleTemplate(particle, particleButton, id, particlePrefab2);
                    if (PlayerPrefs.instance.IsFav(id, shouldLoadPartileFromBundle: false))
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
                    }
                    else
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
                    }
                    isCategoryGenerated = false;
                }
                else
                {
                    particle = catList[0]._myParticleList._particlePrefab[0].Prefab;
                    particleButton = catList[0]._myParticleList._ParticleButtonList[0];
                    int uniqueID = catList[0]._myParticleList._particlePrefab[0].UniqueID;
                    ParticlePrefab _prefab = catList[0]._myParticleList._particlePrefab[0];
                    LoadTemplate.instance.LoadParticleTemplate(particle, particleButton, uniqueID, _prefab);
                    ExportManager.instance.particleToUnlock = id;
                    ParticlePrefab particlePrefab3 = new ParticlePrefab();
                    particlePrefab3 = catList[i]._myParticleList._particlePrefab[j];
                    ExportManager.instance.particlePrefab = particlePrefab3;
                    if (PlayerPrefs.instance.IsFav(id, shouldLoadPartileFromBundle: false))
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
                    }
                    else
                    {
                        SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
                    }
                    isCategoryGenerated = false;
                }
                selectedCategoryBtn = catList[i]._myParticleList._Button;
                SettingManager.instance.SynchronizeLastCategorySelection(selectedCategoryBtn);
            }
        }
        if (isCategoryGenerated)
        {
            particle = catList[0]._myParticleList._particlePrefab[0].Prefab;
            particleButton = catList[0]._myParticleList._ParticleButtonList[0];
            int uniqueID2 = catList[0]._myParticleList._particlePrefab[0].UniqueID;
            SettingManager.instance.previewSystem.SetActive(value: false);
            SettingManager.instance.particleSystem.SetActive(value: true);
            ExportManager.instance.particleToUnlock = uniqueID2;
            ParticlePrefab particlePrefab4 = new ParticlePrefab();
            particlePrefab4 = catList[0]._myParticleList._particlePrefab[0];
            ExportManager.instance.particlePrefab = particlePrefab4;
            if (PlayerPrefs.instance.IsFav(uniqueID2, shouldLoadPartileFromBundle: false))
            {
                SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
            }
            else
            {
                SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
            }
            LoadTemplate.instance.LoadParticleTemplate(particle, particleButton, uniqueID2, particlePrefab4);
        }
        if (SettingManager.instance.selectedTranscation != null)
        {
            UnityEngine.Object.Destroy(SettingManager.instance.selectedTranscation);
        }

        if (themeIdNumber != -1)
        {
            ThemeManager.instance.LoadPrefabTheme(ThemeManager.instance.lstOFThemePrefab[themeIdNumber]._Prefab, ThemeManager.instance.totalTheme[themeIdNumber]);
            return;
        }

        if (!shouldNotLoadRandomTranscation)
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
        else
        {
            TransactionManager.instance.OnClickToTransaction(TransactionManager.instance.lstOfTranscation[0].Prefab, TransactionManager.instance.themeContainers[0]);
        }
    }

}
