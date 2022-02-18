using System.Collections;
using UnityEngine;

public class AudioSyncWithLensFlareBright : AudioSpecturmBase
{
    [Space]
    public float max = 10f;

    public float min;

    [Space]
    public bool _IsAffectedToUserSetting;

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("FlareBrightness");
        StartCoroutine("FlareBrightness", max);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            base.transform.GetComponent<LensFlare>().brightness = Mathf.Lerp(base.transform.GetComponent<LensFlare>().brightness, min, TotalTimeT * Time.deltaTime);
        }
    }

    private IEnumerator FlareBrightness(float _target)
    {
        float num = base.transform.GetComponent<LensFlare>().brightness;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            base.transform.GetComponent<LensFlare>().brightness = num;
            yield return null;
        }
        m_IsBeat = false;
    }

}
