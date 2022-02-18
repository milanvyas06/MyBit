using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager instance;

    public Material matLight;

    public GameObject themeContainer;

    private int globalCounter;

    public AudioSource audioSource;

    public GameObject themes;

    public GameObject content;

    public GameObject previewTheme;

    public ThemePrefabList[] lstOFThemePrefab;
    public List<ThemeSubDetails> prefabEffectDetails = new List<ThemeSubDetails>();

    public List<GameObject> totalTheme = new List<GameObject>();

    public Text txt;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        globalCounter = 0;
        ThemePrefabList[] array = lstOFThemePrefab;
        foreach (ThemePrefabList themePrefabList in array)
        {
            GameObject gameObject = Object.Instantiate(themeContainer, new Vector3(0f, 0f, 0f), Quaternion.identity);
            gameObject.transform.parent = content.transform;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(141f, 250f);
            GameObject gameObject2 = gameObject.transform.GetChild(0).GetChild(0).gameObject;
            Text component = gameObject.transform.GetChild(1).GetChild(0).GetChild(0)
                .gameObject.transform.GetComponent<Text>();
            component.text = themePrefabList.ThemeName;
            GameObject gameObject3 = Object.Instantiate(themePrefabList._Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            gameObject3.transform.parent = gameObject2.transform;
            gameObject3.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
            gameObject3.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            gameObject3.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
            prefabEffectDetails.Add(gameObject3.GetComponent<ThemeSubDetails>());
            totalTheme.Add(gameObject);
            if (DoNotDestroyee.instance.bool2)
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else if (themePrefabList.IsUnlockedDefault)
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else if (PlayerPrefs.instance.IsThemeUnlocked(themePrefabList.id))
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: true);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: false);
            }
            if (themePrefabList._IsNew)
            {
                gameObject.transform.GetChild(3).transform.gameObject.SetActive(value: true);
            }

            gameObject.transform.GetComponent<ThemeButtons>().TransactionButton.onClick.AddListener(delegate
            {
                LoadPrefabTheme(themePrefabList._Prefab, gameObject);
            });
            gameObject.transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.onClick.AddListener(delegate
            {
                OnClickToUnlockTransaction(themePrefabList.id, themePrefabList.ThemeName, themePrefabList._Prefab);
            });

            globalCounter++;
        }
    }

    public void LoadPrefabTheme(GameObject theme, GameObject container)
    {
        if (SettingManager.instance.selectedPrefabTheme != null)
        {
            Object.Destroy(SettingManager.instance.selectedPrefabTheme);
        }
        ExportManager.instance.selectedTheme = theme;
        GameObject gameObject = Object.Instantiate(theme, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = themes.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingManager.instance.selectedPrefabTheme = gameObject;
        SettingManager.instance.ApplyingBeatSettingForTheme(SettingManager.instance.selectedPrefabTheme.GetComponent<ThemeSubDetails>());
        ThemeSubDetails component = SettingManager.instance.selectedPrefabTheme.GetComponent<ThemeSubDetails>();
        for (int i = 0; i < component.lstOfUserImage.Length; i++)
        {
            for (int j = 0; j < component.lstOfUserImage[i]._Images.Length; j++)
            {
                if (component.isTextUnlocked)
                {
                    component.lstOfUserImage[i]._Images[j].material = matLight;
                }
            }
        }
        if (ExportManager.instance.selectedImageSprite != null)
        {
            ThemeSubDetails component2 = SettingManager.instance.selectedPrefabTheme.GetComponent<ThemeSubDetails>();
            for (int k = 0; k < component2.lstOfUserImage.Length; k++)
            {
                for (int l = 0; l < component2.lstOfUserImage[k]._Images.Length; l++)
                {
                    component2.lstOfUserImage[k]._Images[l].sprite = ExportManager.instance.selectedImageSprite;
                    if (component2.isTextUnlocked)
                    {
                        component2.lstOfUserImage[k]._Images[l].material = matLight;
                    }
                }
            }
        }
        component.img.material = matLight;
        ChangeBorder(container);
    }

    public void OnClickToUnlockTransaction(int transcationId, string transcationName, GameObject _prefab)
    {
        Debug.Log("Locked theme is clicked.! " + transcationName);
        SettingManager.instance.unlockThemePanel.transform.GetComponent<Animator>().Play("IN");

        SettingManager.instance.themeName.text = transcationName;
        ExportManager.instance.themeToUnlock = transcationId;
        GameObject gameObject = Object.Instantiate(_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = previewTheme.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        themes.transform.localScale = new Vector3(0f, 0f, 0f);
        previewTheme.transform.localScale = new Vector3(1f, 1f, 1f);


        for (int i = 0; i < gameObject.transform.GetComponent<ThemeSubDetails>().lstOfUserImage.Length; i++)
        {
            for (int j = 0; j < gameObject.transform.GetComponent<ThemeSubDetails>().lstOfUserImage[i]._Images.Length; j++)
            {
                gameObject.transform.GetComponent<ThemeSubDetails>().lstOfUserImage[i]._Images[j].sprite = ExportManager.instance.selectedImageSprite;
            }
        }

        SettingManager.instance.transcationInProcessPrefab = gameObject;
        AudioHelperMain.instance.audioSourceAndroid.Play();
    }

    private void ChangeBorder(GameObject go)
    {
        if (SettingManager.instance.selectedTheme != null)
        {
            SettingManager.instance.selectedTheme.transform.GetChild(2).GetComponent<Image>().sprite = SettingManager.instance.themeUnSelected;
        }
        SettingManager.instance.selectedTheme = go;
        SettingManager.instance.selectedTheme.transform.GetChild(2).GetComponent<Image>().sprite = SettingManager.instance.themeSelected;
    }

    public void ButtonManager(int id)
    {
        Debug.Log(id);
        for (int i = 0; i < totalTheme.Count; i++)
        {
            if (id == i)
            {
                Debug.Log("One Match Found");
            }
        }
    }

}
