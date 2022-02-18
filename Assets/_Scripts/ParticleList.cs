using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ParticleList : MonoBehaviour
{
    public static ParticleList instance;

    public GameObject _ParticleButton;

    private int _IndexNo;

    public AudioSource _Audio;

    public GameObject ParticleParent;

    public GameObject ParticleButtonParent;

    public bool _FirstParticleToBeCreate;

    public GameObject _Button;

    public ParticlePrefab[] _particlePrefab;

    [HideInInspector]
    public List<GameObject> _ParticleButtonList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void GenerateParticleList(string cateName)
    {
        _IndexNo = 0;
        if (_particlePrefab.Length > 0)
        {
            ParticlePrefab[] particlePrefab = _particlePrefab;
            foreach (ParticlePrefab particlePrefab2 in particlePrefab)
            {
                GameObject particleButton = UnityEngine.Object.Instantiate(_ParticleButton, new Vector3(0f, 0f, 0f), Quaternion.identity);
                particleButton.transform.parent = ParticleButtonParent.transform;
                particleButton.transform.localScale = new Vector3(1f, 1f, 1f);
                particleButton.transform.GetComponent<ParticleDataCollector>().Thumbnail.sprite = particlePrefab2._Thumbnail;
                particleButton.transform.GetComponent<ParticleDataCollector>().ParticleName.text = particlePrefab2._Name;
                particleButton.transform.GetComponent<ParticleDataCollector>()._MyCategoryName = cateName;
                if (particlePrefab2._UnlockDefault)
                {
                    particlePrefab2._IsUnlocked = true;
                    particleButton.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                }
                else if (PlayerPrefs.instance.CheckParticleIsUnlock(particlePrefab2.UniqueID))
                {
                    particleButton.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                    particleButton.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: false);
                    particlePrefab2._IsUnlocked = true;
                }
                else if (DoNotDestroyee.instance.isAllUnlockDefault)
                {
                    particleButton.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                    particleButton.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: false);
                }
                else
                {
                    particleButton.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: false);
                    particleButton.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: true);
                }
                if (PlayerPrefs.instance.IsFav(particlePrefab2.UniqueID, shouldLoadPartileFromBundle: false))
                {
                    FavouriteTemplateDetail item = default(FavouriteTemplateDetail);
                    item.ShortingNo = PlayerPrefs.instance.ShortingNo(particlePrefab2.UniqueID, isFavGet: false);
                    item.UniqueNo = particlePrefab2.UniqueID;
                    item._particlePrefab = particlePrefab2;
                    item.ParticleButton = null;
                    item._bundlePath = string.Empty;
                    item.IsBundle = false;
                    CategoryManager.instance.favoriteTemplates.Add(item);
                }
                _ParticleButtonList.Add(particleButton);
                _IndexNo++;
                particleButton.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.onClick.AddListener(delegate
                {
                    SetButtonClickListner(particlePrefab2.Prefab, particleButton, particlePrefab2.UniqueID, particlePrefab2);
                });
                particleButton.transform.GetComponent<ParticleDataCollector>().UnlockButton.onClick.AddListener(delegate
                {
                    SetUnlockButtonListner(particlePrefab2.UniqueID, particlePrefab2.Prefab, particlePrefab2._Name);
                });
            }
        }
        try
        {
            if (!_FirstParticleToBeCreate)
            {
            }
        }
        catch (Exception)
        {
            Debug.Log("No themes to be loaded");
        }
    }

    private void SetButtonClickListner(GameObject particlePrefab, GameObject go, int uId, ParticlePrefab prefab2)
    {
        ExportManager.instance.particleToUnlock = uId;
        ExportManager.instance.particlePrefab = prefab2;
        ExportManager.instance.shouldLoadPartileFromBundle = false;
        if (PlayerPrefs.instance.IsFav(uId, ExportManager.instance.shouldLoadPartileFromBundle))
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[0];
        }
        else
        {
            SettingManager.instance.favParticle.sprite = SettingManager.instance.isFavorite[1];
        }
        LoadTemplate.instance.LoadParticleTemplate(particlePrefab, go, uId, prefab2);
    }

    private void SetUnlockButtonListner(int uid, GameObject prefab, string name)
    {
        ExportManager.instance.particleToUnlock = uid;

        SettingManager.instance.UnlockParticlePanel.transform.GetComponent<Animator>().Play("IN");

        SettingManager.instance.previewSystem.SetActive(value: true);
        SettingManager.instance.particleSystem.SetActive(value: false);
        LoadTemplate.instance.LoadParticleTemplateForPreviewOnly(prefab, uid);

        if (UnlockParticleManager.instance.isLocked)
        {
            for (int i = 0; i < CategoryManager.instance.catList.Length; i++)
            {
                for (int j = 0; j < CategoryManager.instance.catList[i]._myParticleList._particlePrefab.Length; j++)
                {
                    if (uid == CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j].UniqueID)
                    {
                        string categoryName = CategoryManager.instance.catList[i].CategoryName;
                        SettingManager.instance.txtTitlePnlUnlock.text = categoryName;
                        string text = "Unlock " + categoryName + " Premium Particle Effect by Watching Video.";
                        SettingManager.instance.panelDetails.text = text;
                    }
                }
            }
        }
        else
        {
            SettingManager.instance.txtTitlePnlUnlock.text = name;
        }
    }

}
