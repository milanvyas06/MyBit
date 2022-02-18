using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class AudioSyncWithLight : AudioSpecturmBase
{
    [Space]
    public float max;

    public float min;

    [Space]
    public bool _IsAffectedToUserSetting;

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            float intensity = Mathf.Lerp(base.transform.GetComponent<Light>().intensity, min, TotalTimeT * Time.deltaTime);
            base.transform.GetComponent<Light>().intensity = intensity;
        }
    }

    private IEnumerator LightIntensity(float _target)
    {
        float num = base.transform.GetComponent<Light>().intensity;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            base.transform.GetComponent<Light>().intensity = num;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("LightIntensity");
        StartCoroutine("LightIntensity", max);
    }

}
