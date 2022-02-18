using System.Collections;
using UnityEngine;

public class AudioSyncWithParticleNoice : AudioSpecturmBase
{
    [Space]
    public float max;

    public float min;

    [Space]
    public Vector3 _beatNoice;

    public Vector3 _restNoice;

    [Space]
    public bool _IsAffectedToUserSetting;

    private IEnumerator ParticleNoice(float _target, Vector3 _taget2)
    {
        if (!base.transform.GetComponent<ParticleSystem>().noise.separateAxes)
        {
            float num = base.transform.GetComponent<ParticleSystem>().noise.strength.constant;
            float a = num;
            float num2 = 0f;
            while (num != _target)
            {
                num = Mathf.Lerp(a, _target, num2 / timeToBeat);
                num2 += Time.deltaTime;
                ParticleSystem.NoiseModule noise = base.transform.GetComponent<ParticleSystem>().noise;
                noise.strength = num;
                yield return null;
            }
            m_IsBeat = false;
            yield break;
        }
        ParticleSystem.NoiseModule noise2 = base.transform.GetComponent<ParticleSystem>().noise;
        Vector3 vector = new Vector3(noise2.strengthX.constant, noise2.strengthY.constant, noise2.strengthZ.constant);
        Vector3 a2 = vector;
        float num3 = 0f;
        while (vector != _taget2)
        {
            vector = Vector3.Lerp(a2, _taget2, num3 / timeToBeat);
            num3 += Time.deltaTime;
            ParticleSystem.NoiseModule noise3 = base.transform.GetComponent<ParticleSystem>().noise;
            noise3.strengthX = vector.x;
            noise3.strengthY = vector.y;
            noise3.strengthZ = vector.z;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            if (!base.transform.GetComponent<ParticleSystem>().noise.separateAxes)
            {
                ParticleSystem.NoiseModule noise = base.transform.GetComponent<ParticleSystem>().noise;
                float constant = Mathf.Lerp(noise.strength.constant, min, TotalTimeT * Time.deltaTime);
                noise.strength = constant;
                return;
            }
            ParticleSystem.NoiseModule noise2 = base.transform.GetComponent<ParticleSystem>().noise;
            Vector3 a = new Vector3(noise2.strengthX.constant, noise2.strengthY.constant, noise2.strengthZ.constant);
            Vector3 vector = Vector3.Lerp(a, _restNoice, TotalTimeT * Time.deltaTime);
            noise2.strengthX = vector.x;
            noise2.strengthY = vector.y;
            noise2.strengthZ = vector.z;
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("ParticleNoice");
        StartCoroutine(ParticleNoice(max, _beatNoice));
    }
}
