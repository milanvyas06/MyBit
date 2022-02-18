using UnityEngine;

public class UnlockParticleManager : MonoBehaviour
{
    public static UnlockParticleManager instance;

    public bool isLocked;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        unlockParticle(8);
    }
    public void unlockParticle(int unlockParticleId)
    {
        if (PlayerPrefs.instance.UnlockParticle(unlockParticleId))
        {
            if (!isLocked)
            {
                for (int i = 0; i < CategoryManager.instance.catList.Length; i++)
                {
                    for (int j = 0; j < CategoryManager.instance.catList[i]._myParticleList._particlePrefab.Length; j++)
                    {
                        if (unlockParticleId == CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j].UniqueID)
                        {
                            GameObject prefab = CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j].Prefab;
                            GameObject gameObject = CategoryManager.instance.catList[i]._myParticleList._ParticleButtonList[j];
                            CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j]._IsUnlocked = true;
                            gameObject.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                            gameObject.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: false);
                            LoadTemplate.instance.LoadParticleTemplate(prefab, gameObject, unlockParticleId, CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j]);
                        }
                    }
                }
                return;
            }
            for (int k = 0; k < CategoryManager.instance.catList.Length; k++)
            {
                for (int l = 0; l < CategoryManager.instance.catList[k]._myParticleList._particlePrefab.Length; l++)
                {
                    if (unlockParticleId == CategoryManager.instance.catList[k]._myParticleList._particlePrefab[l].UniqueID)
                    {
                        int num = k;
                        for (int m = 0; m < CategoryManager.instance.catList[k]._myParticleList._particlePrefab.Length; m++)
                        {
                            ParticlePrefab particlePrefab = CategoryManager.instance.catList[k]._myParticleList._particlePrefab[m];
                            int uniqueID = particlePrefab.UniqueID;
                            PlayerPrefs.instance.UnlockParticle(uniqueID);
                            GameObject gameObject2 = CategoryManager.instance.catList[k]._myParticleList._ParticleButtonList[m];
                            gameObject2.transform.GetComponent<ParticleDataCollector>().SelectParticleButton.transform.gameObject.SetActive(value: true);
                            gameObject2.transform.GetComponent<ParticleDataCollector>().UnlockButton.transform.gameObject.SetActive(value: false);
                        }
                        ParticlePrefab myparticlePrefab = CategoryManager.instance.catList[k]._myParticleList._particlePrefab[l];
                        GameObject prefab2 = CategoryManager.instance.catList[k]._myParticleList._particlePrefab[l].Prefab;
                        GameObject particleBtn = CategoryManager.instance.catList[k]._myParticleList._ParticleButtonList[l];
                        CategoryManager.instance.catList[k]._myParticleList._particlePrefab[l]._IsUnlocked = true;
                        LoadTemplate.instance.LoadParticleTemplate(prefab2, particleBtn, unlockParticleId, myparticlePrefab);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Truble to unlock particle");
        }
    }


    public void UnlockParticalsFromAndroid(int particalId)
    {
        Debug.Log(" Unlocking particle !!!! :- " + particalId);
        unlockParticle(particalId);
    }

    public bool IsUnlockedAllParticle()
    {
        int num = 0;
        for (int i = 0; i < CategoryManager.instance.catList.Length; i++)
        {
            for (int j = 0; j < CategoryManager.instance.catList[i]._myParticleList._particlePrefab.Length; j++)
            {
                if (!CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j]._UnlockDefault && PlayerPrefs.instance.CheckParticleIsUnlock(CategoryManager.instance.catList[i]._myParticleList._particlePrefab[j].UniqueID).ToString().Equals("False"))
                {
                    num++;
                }
            }
        }
        if (num <= 0)
        {
            return true;
        }
        return false;
    }
}
