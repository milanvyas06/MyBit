using UnityEngine;
using UnityEngine.UI;

public class NumberFormaterToF2 : MonoBehaviour
{
    public static NumberFormaterToF2 instance;

    public Text txtToUpdate;

    public float num = 0.5f;

    private float globalVal;

    private int numCounter;

    private float numLocal;

    public string str;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numLocal = num;
    }
    private void Update()
    {
        numLocal -= Time.deltaTime;
        globalVal += Time.timeScale / Time.deltaTime;
        numCounter++;
        if ((double)numLocal <= 0.0)
        {
            str = string.Empty + (globalVal / (float)numCounter).ToString("f2");
            numLocal = num;
            globalVal = 0f;
            numCounter = 0;
        }
        txtToUpdate.text = str.ToString();
    }

}
