using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTheme : MonoBehaviour
{
    public bool shouldUnLockAllTheme;

    public static UnlockTheme instance;

    private void Start()
    {
        instance = this;
    }

    public void UnlockThemeFromAndroid(int id)
    {
        Debug.Log("Unlocking Theme !!! : " + id);
        UnlockAllTheme(id);
    }

    public void UnlockAllTheme(int number)
    {
        if (shouldUnLockAllTheme)
        {
            for (int i = 0; i < ThemeManager.instance.lstOFThemePrefab.Length; i++)
            {
                PlayerPrefs.instance.UnlockTheme(ThemeManager.instance.lstOFThemePrefab[i].id);
                ThemeManager.instance.totalTheme[i].transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.transform.gameObject.SetActive(value: false);
                ThemeManager.instance.totalTheme[i].transform.GetComponent<ThemeButtons>().TransactionButton.transform.gameObject.SetActive(value: false);
            }
            GameObject prefab = ThemeManager.instance.lstOFThemePrefab[number]._Prefab;
            GameObject selectedTransaction = ThemeManager.instance.totalTheme[number];
            ThemeManager.instance.LoadPrefabTheme(prefab, selectedTransaction);
            return;
        }
        PlayerPrefs.instance.UnlockTheme(number);
        for (int j = 0; j < ThemeManager.instance.lstOFThemePrefab.Length; j++)
        {
            if (number == ThemeManager.instance.lstOFThemePrefab[j].id)
            {
                ThemeManager.instance.totalTheme[j].transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.transform.gameObject.SetActive(value: false);
                ThemeManager.instance.totalTheme[j].transform.GetComponent<ThemeButtons>().TransactionButton.transform.gameObject.SetActive(value: true);
                GameObject prefab2 = ThemeManager.instance.lstOFThemePrefab[j]._Prefab;
                GameObject selectedTransaction = ThemeManager.instance.totalTheme[j];
                ThemeManager.instance.LoadPrefabTheme(prefab2, selectedTransaction);
            }
        }
    }


    public bool IsUnlockedAllTheme()
    {
        int num = 0;
        for (int i = 0; i < ThemeManager.instance.lstOFThemePrefab.Length; i++)
        {
            if (!ThemeManager.instance.lstOFThemePrefab[i].IsUnlockedDefault && PlayerPrefs.instance.CheckIsThemeUnlock(ThemeManager.instance.lstOFThemePrefab[i].id).ToString().Equals("False"))
            {
                num++;
            }
        }
        if (num <= 0)
        {
            return true;
        }
        return false;
    }
}
