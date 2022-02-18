using UnityEngine;

public class DoNotDestroyee : MonoBehaviour
{
    public static DoNotDestroyee instance;

    public int num1;

    public int num2;

    public bool bool1;

    public bool isAllUnlockDefault;

    public bool bool2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Object.DontDestroyOnLoad(base.transform.gameObject);
        }
        else
        {
            Object.DestroyImmediate(base.transform.gameObject);
        }
    }
}
