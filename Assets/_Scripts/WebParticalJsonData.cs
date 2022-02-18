using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WebParticalJsonData : MonoBehaviour
{
    public class CategoryData
    {
        public Sprite sprite;

        public bool isItNewCategory = true;
    }

    [Serializable]
    public class WaitingForDecryptedData
    {
        public string UniqueNo;

        public string ThumbnailPath;

        public string ThemeName;

        public string PrefabName;

        public GameObject ParticleButton;

        public string DownloadAt;

        public string NewFlag;


        public void MakeNullObj()
        {
            UniqueNo = string.Empty;
            ThumbnailPath = string.Empty;
            ThemeName = string.Empty;
            PrefabName = string.Empty;
            ParticleButton = null;
        }

    }

    public class AssetBundleHelper
    {
        public bool isDecyptPathAvilable;

        public string path;
    }

    public static WebParticalJsonData instance;

    public ParticleJsonData particleJsonData;

    [Space]
    public GameObject Particle1;

    [Space]
    public GameObject MoreThemes;

    [Space]
    public bool setAsFirstSibling;

    public List<string> lstOfUiD;

    [HideInInspector]
    public WaitingForDecryptedData decryptHelper = new WaitingForDecryptedData();


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (Application.platform != RuntimePlatform.Android) GetStringFromFile();
    }

    private void GetStringFromFile()
    {
        var jsonTextFile = Resources.Load<TextAsset>("TextJson/ParticleJsonOnLoad");
        LoadJsonParticalData(jsonTextFile.text);
    }


    public void LoadJsonParticalData(string str)
    {
        Debug.Log("Reteive Online web partical data :- " + str);
        particleJsonData = JsonUtility.FromJson<ParticleJsonData>(str);
        for (int i = 0; i < particleJsonData.ParticalDetails.Count; i++)
        {
            bool isCategoryExist = false;
            for (int j = 0; j < CategoryManager.instance.catRegisterd.Count; j++)
            {
                if (particleJsonData.ParticalDetails[i].ParticleCatName == CategoryManager.instance.catRegisterd[j].catName)
                {
                    if (CategoryManager.instance.catRegisterd[j].isGeneratedFromJson)
                    {
                        AddToParticleList(particleJsonData.ParticalDetails[i].ParticalInfo, particleJsonData.ParticalDetails[i].ParticleCatName, CategoryManager.instance.catRegisterd[j]._JsonCategoryInformation.ParticleButtonParent, CategoryManager.instance.catRegisterd[j]._JsonCategoryInformation.ParticleButton);
                    }
                    else
                    {
                        AddToParticleList(particleJsonData.ParticalDetails[i].ParticalInfo, particleJsonData.ParticalDetails[i].ParticleCatName, CategoryManager.instance.catRegisterd[j]._CatList._myParticleList.ParticleButtonParent, CategoryManager.instance.catRegisterd[j]._CatList._myParticleList._ParticleButton);
                    }

                    isCategoryExist = true;
                }
            }
            if (!isCategoryExist)
            {
                CreateNewCategory(particleJsonData.ParticalDetails[i].ParticleCatName, CategoryManager.instance.categoryButton, CategoryManager.instance.content, CategoryManager.instance.categoryContainer, CategoryManager.instance.particleScrollerMask, particleJsonData.ParticalDetails[i].ParticalCatImg, CategoryManager.instance.icon, CategoryManager.instance.tempImg, particleJsonData.ParticalDetails[i].ParticalInfo);
            }
        }
    }

    public void LoadAssetBundle(string bundlePath)
    {
        LoadTemplate.instance.LoadParticleAssetBundle(decryptHelper.UniqueNo, bundlePath, decryptHelper.ThumbnailPath, decryptHelper.ThemeName, decryptHelper.PrefabName, decryptHelper.ParticleButton);
        decryptHelper.MakeNullObj();
    }

    private AssetBundleHelper LoadAssetBundles(string UId)
    {
        AssetBundleHelper assetBundle = new AssetBundleHelper();
        for (int i = 0; i < particleJsonData.ParticalDetails.Count; i++)
        {
            for (int j = 0; j < particleJsonData.ParticalDetails[i].ParticalInfo.Count; j++)
            {
                if (particleJsonData.ParticalDetails[i].ParticalInfo[j].UniqueIDNo == UId)
                {
                    string decryptedBundlePath = particleJsonData.ParticalDetails[i].ParticalInfo[j].DecryptedBundlePath;
                    if (decryptedBundlePath == null)
                    {
                        assetBundle.isDecyptPathAvilable = false;
                        assetBundle.path = string.Empty;
                    }
                    else
                    {
                        assetBundle.isDecyptPathAvilable = true;
                        assetBundle.path = particleJsonData.ParticalDetails[i].ParticalInfo[j].DecryptedBundlePath;
                    }
                }
            }
        }
        return assetBundle;
    }

    public void ClickOnBackCategory(GameObject go)
    {
        SettingManager.instance.catName = string.Empty;
        go.transform.GetComponent<Animator>().Play("GeneralOUT");
        CategoryManager.instance.category.transform.GetComponent<Animator>().Play("CategoryIN");
    }

    public CategoryData CategoryIcon(string str)
    {
        CategoryData catData = new CategoryData();
        bool flag = false;
        for (int i = 0; i < CategoryManager.instance.newCategorySpriteRegister.Count; i++)
        {
            if (str == CategoryManager.instance.newCategorySpriteRegister[i].CategoryName)
            {
                CategoryData singleCat = new CategoryData();
                singleCat.sprite = CategoryManager.instance.newCategorySpriteRegister[i].CategorySprite;
                singleCat.isItNewCategory = true;
                catData = singleCat;
                flag = true;
            }
        }
        if (!flag)
        {
            catData.sprite = null;
            catData.isItNewCategory = false;
        }
        return catData;
    }

    private void SetButtonParticleClickForWebParticles(string uId, string particlePathFront, string thumbPath, string themeName, string prefabName, GameObject pButton, string downloadaT, string isNew)
    {
        Debug.Log("Particl Path is :- " + particlePathFront + "prefabName :" + prefabName);
        AssetBundleHelper asetBundleHelper = new AssetBundleHelper();
        asetBundleHelper = LoadAssetBundles(uId);
        ExportManager.instance.particleToUnlock = int.Parse(uId);
        ExportManager.instance.shouldLoadPartileFromBundle = pButton.transform.GetComponent<ParticleDataCollector>()._IsBundle;
        ExportManager.instance.thumbPath = thumbPath;
        ExportManager.instance.particlePath = particlePathFront;
        ExportManager.instance.prefabName = prefabName;
        ExportManager.instance.themeName = themeName;
        if (PlayerPrefs.instance.IsFav(int.Parse(uId), ExportManager.instance.shouldLoadPartileFromBundle))
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
        }
        else
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
        }
        if (asetBundleHelper.isDecyptPathAvilable)
        {
            LoadTemplate.instance.LoadParticleAssetBundle(uId, asetBundleHelper.path, thumbPath, themeName, prefabName, pButton);
            LoadTemplate.instance.changeBorderSprite(pButton);
        }
        else
        {
            decryptHelper.UniqueNo = uId;
            decryptHelper.ThumbnailPath = thumbPath;
            decryptHelper.ThemeName = themeName;
            decryptHelper.PrefabName = prefabName;
            decryptHelper.ParticleButton = pButton;
            decryptHelper.DownloadAt = downloadaT;
            decryptHelper.NewFlag = isNew;
            try
            {
                string text = particlePathFront + "?0";
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
        LoadTemplate.instance.changeBorderSprite(pButton);
    }

    public void CreateNewCategory(string catName, GameObject categoryButton, GameObject parent, GameObject categoryContainer, GameObject objectoCreate1Parent, string thumbnailPath, Sprite icon, Sprite sprite, List<ParticalInfo> lstOfParticle)
    {
        GameObject catBtn = UnityEngine.Object.Instantiate(categoryButton, new Vector3(0f, 0f, 0f), Quaternion.identity);
        GameObject catContainer = UnityEngine.Object.Instantiate(categoryContainer, new Vector3(0f, 0f, 0f), Quaternion.identity);
        catBtn.transform.parent = parent.transform;
        catContainer.transform.parent = objectoCreate1Parent.transform;
        catBtn.transform.localPosition = new Vector3(0f, 0f, 0f);
        catContainer.transform.localPosition = new Vector3(0f, 0f, 0f);
        catBtn.transform.localScale = new Vector3(1f, 1f, 1f);
        catContainer.transform.localScale = new Vector3(1f, 1f, 1f);
        catContainer.transform.name = catName + "_Container";
        CategoryButtonDetail categoryButtonDetails = catBtn.transform.GetComponent<CategoryButtonDetail>();
        CategoryData catData = new CategoryData();
        catData = CategoryIcon(catName);

        if (catData.isItNewCategory)
        {
            categoryButtonDetails.catIcon.sprite = catData.sprite;
        }
        else
        {
            categoryButtonDetails.catIcon.sprite = CategoryManager.instance.catIcon;
        }

        categoryButtonDetails.catName = catName;
        categoryButtonDetails.name = catName;
        categoryButtonDetails.txt.text = catName;
        categoryButtonDetails.catIcon.transform.gameObject.SetActive(value: true);
        StartCoroutine(LoadTexture(thumbnailPath, categoryButtonDetails.catIcon));
        categoryButtonDetails.transform.GetComponent<Image>().sprite = icon;
        catBtn.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            ClickOnCategory(catName, catContainer, catBtn);
        });
        CategoryContainer component2 = catContainer.transform.GetComponent<CategoryContainer>();

        component2.backButton.onClick.AddListener(delegate
        {
            ClickOnBackCategory(catContainer);
        });
        AddToParticleList(lstOfParticle, catName, component2.content, Particle1);
        CategoryRegister.JSonCategoryInformation jSonCategoryInformation = new CategoryRegister.JSonCategoryInformation();
        jSonCategoryInformation.ParticleButtonParent = component2.content;
        jSonCategoryInformation.ParticleButton = Particle1;

        CategoryManager.instance.lstOfGeneratedContainer.Add(catContainer);

        CategoryManager.instance.CreateMoreButton(component2.content, catName, isGeneratedJson: true, catContainer, catBtn, null, jSonCategoryInformation);

        catContainer.GetComponent<CategoryContainer>().animator.Play("GeneralOUT");
        catBtn.GetComponent<Image>().sprite = SettingManager.instance.selectUnSelectCategory[1];
    }

    private bool IsExist(string str, List<string> lstOfId)
    {
        bool result = false;
        for (int i = 0; i < lstOfId.Count; i++)
        {
            if (str == lstOfId[i])
            {
                result = true;
            }
        }
        return result;
    }

    private IEnumerator LoadTexture(string path, Image img)
    {
        if (path != null && path != string.Empty)
        {
            bool flag = false;
            byte[] array = null;
            Texture2D texture2D = null;
            WWW wWW = new WWW("file:///" + path);
            while (!wWW.isDone)
            {
                yield return null;
            }
            try
            {
                texture2D = wWW.texture;
            }
            catch (Exception arg)
            {
                Debug.Log("Texture Loading Exception : " + arg);
            }
            try
            {
                if (texture2D != null)
                {
                    Sprite sprite2 = img.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
                }
            }
            catch (Exception arg2)
            {
                Debug.Log("Failed to convert sprite : " + arg2);
            }
        }
    }


    public void AddToParticleList(List<ParticalInfo> lstOfParticles, string catName, GameObject parentGO, GameObject _prefab)
    {
        List<GameObject> list = new List<GameObject>();
        List<string> lstOfAvailableParticleId = new List<string>();
        for (int i = 0; i < lstOfParticles.Count; i++)
        {
            for (int j = 0; j < lstOfUiD.Count; j++)
            {
                if (lstOfUiD[j] == lstOfParticles[i].UniqueIDNo)
                {
                    lstOfAvailableParticleId.Add(lstOfParticles[i].UniqueIDNo);
                }
            }
        }
        foreach (ParticalInfo current in lstOfParticles)
        {
            if (!IsExist(current.UniqueIDNo, lstOfAvailableParticleId))
            {
                lstOfUiD.Add(current.UniqueIDNo);
                GameObject gameObject = UnityEngine.Object.Instantiate(_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                gameObject.transform.parent = parentGO.transform;
                list.Add(gameObject);
                gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                ParticleDataCollector component = gameObject.transform.GetComponent<ParticleDataCollector>();
                gameObject.transform.GetComponent<ParticleDataCollector>()._MyCategoryName = catName;
                component.ParticleName.text = current.ThemeName;
                component._IsBundle = true;
                component._ThumbnailPath = current.ImgPath;
                gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                gameObject.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: false);
                if (current.NewFlag == "0")
                {
                    gameObject.transform.GetComponent<ParticleDataCollector>()._NewTag.transform.gameObject.SetActive(value: true);
                }
                else
                {
                    gameObject.transform.GetComponent<ParticleDataCollector>()._NewTag.transform.gameObject.SetActive(value: false);
                }
                if (PlayerPrefs.instance.IsFav(int.Parse(current.UniqueIDNo), shouldLoadPartileFromBundle: true))
                {
                    FavouriteTemplateDetail item = default(FavouriteTemplateDetail);
                    item.ShortingNo = PlayerPrefs.instance.ShortingNo(int.Parse(current.UniqueIDNo), isFavGet: true);
                    item.UniqueNo = int.Parse(current.UniqueIDNo);
                    item.ParticleButton = null;
                    item._particlePrefab = null;
                    item._bundlePath = current.BundlePath;
                    item._thumbnailPath = current.ImgPath;
                    item._PrefabName = current.prefbName;
                    item._TemplateName = current.ThemeName;
                    item.IsBundle = true;
                    CategoryManager.instance.favoriteTemplates.Add(item);
                }
                gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.onClick.AddListener(delegate
                {
                    SetButtonParticleClickForWebParticles(current.UniqueIDNo, current.BundlePath, current.ImgPath, current.ThemeName, current.prefbName, gameObject, current.downloadAt, current.NewFlag);
                });
                gameObject.transform.GetComponent<ParticleDataCollector>().UnlockButton.onClick.AddListener(delegate
                {
                });
            }
        }
        if (setAsFirstSibling)
        {
            for (int num = list.Count - 1; num >= 0; num--)
            {
                RectTransform component2 = list[num].transform.GetComponent<RectTransform>();
                component2.SetAsFirstSibling();
            }
        }
        if (!setAsFirstSibling)
        {
        }
    }

    public void ClickOnCategory(string catName, GameObject categoryPanel, GameObject categoryButton)
    {
        SettingManager.instance.catName = catName;
        categoryPanel.transform.GetComponent<Animator>().Play("GeneralIN");
        CategoryManager.instance.category.transform.GetComponent<Animator>().Play("CategoryOUT");
        SettingManager.instance.SynchronizeLastCategorySelection(categoryButton);
    }
}
