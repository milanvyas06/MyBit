using System;
using UnityEngine;

public class SetWatermarkPos : MonoBehaviour
{
    public static SetWatermarkPos instance;

    [Space]
    public GameObject[] topSide;

    [Space]
    public GameObject[] bottomSide;

    [Space]
    public GameObject[] allWatermark;

    [Space]
    public GameObject topLeft;

    [Space]
    public GameObject topRight;

    [Space]
    public GameObject bottomLeft;

    [Space]
    public GameObject bottomRight;

    private void Start()
    {
        string text = string.Empty;
        try
        {
            text = ExportManager.instance.selectedParticle.transform.GetComponent<ParticleData>()._WaterMarkPosition.ToString();
        }
        catch (Exception)
        {
        }
        if (text.Equals("TopRandom"))
        {
            int num = UnityEngine.Random.Range(0, topSide.Length);
            topSide[num].SetActive(value: true);
        }
        if (text.Equals("BottomRandom"))
        {
            int num2 = UnityEngine.Random.Range(0, bottomSide.Length);
            bottomSide[num2].SetActive(value: true);
        }
        if (text.Equals("Random"))
        {
            int num3 = UnityEngine.Random.Range(0, allWatermark.Length);
            allWatermark[num3].SetActive(value: true);
        }
        if (text.Equals("TopLeft"))
        {
            topLeft.SetActive(value: true);
        }
        if (text.Equals("TopRight"))
        {
            topRight.SetActive(value: true);
        }
        if (text.Equals("BottomLeft"))
        {
            bottomLeft.SetActive(value: true);
        }
        if (text.Equals("BottomRight"))
        {
            bottomRight.SetActive(value: true);
        }
    }

    private void Awake()
    {
        instance = this;
    }

}
