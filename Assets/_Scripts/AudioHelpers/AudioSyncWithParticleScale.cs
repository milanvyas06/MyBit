using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class AudioSyncWithParticleScale : AudioSpecturmBase
{
    [Space]
    public float max;

    public float min;

    [Space]
    private float current;

    [Space]
    public bool _IsAffectedToUserSetting;

    private IEnumerator SpwamSize(float _target)
    {
        float num = current;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            current = num;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        ParticleSystemRenderer component = GetComponent<ParticleSystemRenderer>();
        component.minParticleSize = current;
        base.OnUpdate();
        if (!m_IsBeat)
        {
            float num = current = Mathf.Lerp(current, min, TotalTimeT * Time.deltaTime);
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("SpwamSize");
        StartCoroutine("SpwamSize", max);
    }

}
