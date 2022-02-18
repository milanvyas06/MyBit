using UnityEngine;

public class UnlockTransaction : MonoBehaviour
{
    public bool shouldUnlockAllTranscation;

    public static UnlockTransaction instance;

    private void Awake()
    {
        instance = this;
    }


    public void UnlockTransactionFromAndroid(int id)
    {
        Debug.Log(" Unlocking Theme !!! : " + id);
        UnlockAllTransaction(id);
    }

    public void UnlockAllTransaction(int number)
    {
        if (shouldUnlockAllTranscation)
        {
            for (int i = 0; i < TransactionManager.instance.lstOfTranscation.Count; i++)
            {
                PlayerPrefs.instance.UnlockTransaction(TransactionManager.instance.lstOfTranscation[i].id);
                TransactionManager.instance.themeContainers[i].transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.transform.gameObject.SetActive(value: false);
                TransactionManager.instance.themeContainers[i].transform.GetComponent<ThemeButtons>().TransactionButton.transform.gameObject.SetActive(value: false);
            }
            GameObject prefab = TransactionManager.instance.lstOfTranscation[number].Prefab;
            GameObject selectedTransaction = TransactionManager.instance.themeContainers[number];
            TransactionManager.instance.OnClickToTransaction(prefab, selectedTransaction);
            return;
        }
        PlayerPrefs.instance.UnlockTransaction(number);
        for (int j = 0; j < TransactionManager.instance.lstOfTranscation.Count; j++)
        {
            if (number == TransactionManager.instance.lstOfTranscation[j].id)
            {
                TransactionManager.instance.themeContainers[j].transform.GetComponent<ThemeButtons>().UnLockButtonTransaction.transform.gameObject.SetActive(value: false);
                TransactionManager.instance.themeContainers[j].transform.GetComponent<ThemeButtons>().TransactionButton.transform.gameObject.SetActive(value: true);
                GameObject prefab2 = TransactionManager.instance.lstOfTranscation[j].Prefab;
                GameObject selectedTransaction = TransactionManager.instance.themeContainers[j];
                TransactionManager.instance.OnClickToTransaction(prefab2, selectedTransaction);
            }
        }
    }

    public bool IsUnlockedAllTransition()
    {
        int num = 0;
        for (int i = 0; i < TransactionManager.instance.lstOfTranscation.Count; i++)
        {
            if (!TransactionManager.instance.lstOfTranscation[i].IsUnlockedDefault && PlayerPrefs.instance.CheckIsTransitionUnlock(TransactionManager.instance.lstOfTranscation[i].id).ToString().Equals("False"))
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
