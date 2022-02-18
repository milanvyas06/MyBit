using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManager : MonoBehaviour
{
    public static TransactionManager instance;

    public GameObject themeContainer;

    public GameObject themeParent;

    public GameObject theme;

    public GameObject previewTheme;

    public List<Transactions> lstOfTranscation;

    [HideInInspector]
    public List<GameObject> prefabEffectDetails;

    public List<GameObject> themeContainers;

    private int globalCounter;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transactions current in lstOfTranscation)
        {
            GameObject gameObject = Object.Instantiate(themeContainer, new Vector3(0f, 0f, 0f), Quaternion.identity);
            gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>().text = current.TransactionName;
            gameObject.transform.parent = themeParent.transform;
            gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject gameObject2 = Object.Instantiate(current.Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            gameObject2.transform.parent = gameObject.transform.GetChild(0).transform.GetChild(0).transform;
            gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
            gameObject2.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            gameObject2.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            prefabEffectDetails.Add(gameObject2);
            themeContainers.Add(gameObject);
            if (DoNotDestroyee.instance.bool2)
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else if (current.IsUnlockedDefault)
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else if (PlayerPrefs.instance.IsTransactionUnlocked(current.id))
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: true);
            }
            else
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(value: true);
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(value: false);
            }
            if (current.IsNew)
            {
                gameObject.transform.GetChild(3).transform.gameObject.SetActive(value: true);
            }
            gameObject.transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.onClick.AddListener(delegate
            {
                OnClickToUnlockTransaction(current.id, current.TransactionName, current.Prefab);
            });
            globalCounter++;
            gameObject.transform.GetComponent<ThemeButtons>().TransactionButton.onClick.AddListener(delegate
            {
                OnClickToTransaction(current.Prefab, gameObject);
            });
        }
    }

    public List<float> ManageTransaction(int totalImages, AudioClip audio)
    {
        List<float> list = new List<float>();
        float num = totalImages;
        float length = audio.length;
        float item = length / num;
        list.Add(num);
        list.Add(length);
        list.Add(item);
        return list;
    }

    public void RestartTransactionInPrafab()
    {
        if (SettingManager.instance.selectedTranscation != null)
        {
            SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>().animator.Play("Image0");
            SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>().isItLastImage = false;
            SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>().SetUpAnimationData(instance.ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
            for (int i = 0; i < 5; i++)
            {
                string name = "Image" + i;
                SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>().animator.SetBool(name, value: false);
            }
        }
        if (SettingManager.instance.transcationInProcessPrefab != null)
        {
            SettingManager.instance.transcationInProcessPrefab.transform.GetComponent<ImageSetupManager>().animator.Play("Image0");
            SettingManager.instance.transcationInProcessPrefab.transform.GetComponent<ImageSetupManager>().isItLastImage = false;
            SettingManager.instance.transcationInProcessPrefab.transform.GetComponent<ImageSetupManager>().SetUpAnimationData(instance.ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
            for (int j = 0; j < 5; j++)
            {
                string name2 = "Image" + j;
                SettingManager.instance.transcationInProcessPrefab.transform.GetComponent<ImageSetupManager>().animator.SetBool(name2, value: false);
            }
        }
    }

    private void ChangeBorderSprite(GameObject go)
    {
        if (SettingManager.instance.selectedThemeContainer != null)
        {
            SettingManager.instance.selectedThemeContainer.transform.GetChild(2).GetComponent<Image>().sprite = SettingManager.instance.themeUnSelected;
        }
        SettingManager.instance.selectedThemeContainer = go;
        SettingManager.instance.selectedThemeContainer.transform.GetChild(2).GetComponent<Image>().sprite = SettingManager.instance.themeSelected;
    }

    public void OnClickToTransaction(GameObject transaction, GameObject selectedTrasncationGo)
    {
        if (SettingManager.instance.selectedPrefabTheme != null)
        {
            Object.Destroy(SettingManager.instance.selectedPrefabTheme);
        }
        if (SettingManager.instance.selectedTranscation != null)
        {
            Object.Destroy(SettingManager.instance.selectedTranscation);
        }
        ExportManager.instance.transactionData = transaction;
        GameObject gameObject = Object.Instantiate(transaction, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = theme.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingManager.instance.selectedTranscation = gameObject;
        ImageSetupManager component = SettingManager.instance.selectedTranscation.transform.GetComponent<ImageSetupManager>();
        for (int i = 0; i < component.lstOFImage.Count; i++)
        {
            for (int j = 0; j < component.lstOFImage[i]._Image.Count; j++)
            {
                if (i < ImageLoader.instance.pathSprites.Count)
                {
                    component.lstOFImage[i]._Image[j].sprite = ImageLoader.instance.pathSprites[i];
                }
            }
        }
        component.SetUpAnimationData(ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
        RestartTransactionInPreview();
        ChangeBorderSprite(selectedTrasncationGo);
        AudioHelperMain.instance.audioSourceAndroid.Play();
    }

    public void OnClickToUnlockTransaction(int transcationId, string transcationName, GameObject _prefab)
    {
        Debug.Log("Found On Unlock Tranction Click event.!");
        SettingManager.instance.unlockTranscationPanel.transform.GetComponent<Animator>().Play("IN");
        ExportManager.instance.themeToUnlock = transcationId;
        SettingManager.instance.transcationName.text = transcationName;
        GameObject gameObject = Object.Instantiate(_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = previewTheme.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        theme.transform.localScale = new Vector3(0f, 0f, 0f);
        previewTheme.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingManager.instance.transcationInProcessPrefab = gameObject;
        ImageSetupManager component = SettingManager.instance.transcationInProcessPrefab.transform.GetComponent<ImageSetupManager>();
        for (int i = 0; i < component.lstOFImage.Count; i++)
        {
            for (int j = 0; j < component.lstOFImage[i]._Image.Count; j++)
            {
                if (i < ImageLoader.instance.pathSprites.Count)
                {
                    component.lstOFImage[i]._Image[j].sprite = ImageLoader.instance.pathSprites[i];
                }
            }
        }
        component.SetUpAnimationData(ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
        RestartTransactionInPreview();
        AudioHelperMain.instance.audioSourceAndroid.Play();

    }

    public void RestartTransactionInPreview()
    {
        for (int i = 0; i < prefabEffectDetails.Count; i++)
        {
            prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().SetUpAnimationData(ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
            prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().animator.Play("Image0");
            prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().SetUpAnimationData(ManageTransaction(ImageLoader.instance.pathSprites.Count, AudioHelperMain.instance.audioSourceAndroid.clip));
            for (int j = 0; j < 5; j++)
            {
                string name = "Image" + j;
                prefabEffectDetails[i].transform.GetComponent<ImageSetupManager>().animator.SetBool(name, value: false);
            }
        }
    }


}
