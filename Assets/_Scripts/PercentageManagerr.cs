using UnityEngine;
using UnityEngine.UI;

public class PercentageManagerr : MonoBehaviour
{
    public Text txt;

    private string currentPercentageVal = string.Empty;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void changeImage()
    {
    }

    public void UpdateProgressVal(string str)
    {
        currentPercentageVal = str;
        int num = int.Parse(str);
        if (num <= 100)
        {
            txt.text = str + " %";
        }
    }

}
