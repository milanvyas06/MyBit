using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AudioSyncWithImageFillAmount : AudioSpecturmBase
{
    [Space]
    public float max = 1f;

    public float min = 0.1f;

    [Space]
    public bool _IsAffectedToUserSetting;

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("ImageFill");
        StartCoroutine("ImageFill", max);
    }

    private IEnumerator ImageFill(float _target)
    {
        float num = base.transform.GetComponent<Image>().fillAmount;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            base.transform.GetComponent<Image>().fillAmount = num;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            base.transform.GetComponent<Image>().fillAmount = Mathf.Lerp(base.transform.GetComponent<Image>().fillAmount, min, TotalTimeT * Time.deltaTime);
        }
    }

}
