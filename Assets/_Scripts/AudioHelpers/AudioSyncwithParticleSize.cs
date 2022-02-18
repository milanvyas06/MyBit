using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AudioSyncwithParticleSize : AudioSpecturmBase
{
    [Space]
    public Axis axis;

    [Space]
    public float max;

    public float min;

    [Space]
    public bool _IsAffectedToUserSetting;


    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("ParticleSimmulation");
        StartCoroutine("ParticleSimmulation", max);
    }

    private IEnumerator ParticleSimmulation(float _target)
    {
        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
        string a = axis.ToString();
        if (a == "x")
        {
            float num = sizeOverLifetime.xMultiplier;
            float a2 = num;
            float num2 = 0f;
            while (num != _target)
            {
                num = Mathf.Lerp(a2, _target, num2 / timeToBeat);
                num2 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime2 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime2.xMultiplier = num;
                yield return null;
            }
            m_IsBeat = false;
        }
        if (a == "y")
        {
            float num3 = sizeOverLifetime.yMultiplier;
            float a3 = num3;
            float num4 = 0f;
            while (num3 != _target)
            {
                num3 = Mathf.Lerp(a3, _target, num4 / timeToBeat);
                num4 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime3 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime3.yMultiplier = num3;
                yield return null;
            }
            m_IsBeat = false;
        }
        if (a == "z")
        {
            float num5 = sizeOverLifetime.zMultiplier;
            float a4 = num5;
            float num6 = 0f;
            while (num5 != _target)
            {
                num5 = Mathf.Lerp(a4, _target, num6 / timeToBeat);
                num6 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime4 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime4.zMultiplier = num5;
                yield return null;
            }
            m_IsBeat = false;
        }
        if (a == "XYZ")
        {
            float num7 = sizeOverLifetime.xMultiplier;
            float num8 = sizeOverLifetime.yMultiplier;
            float num9 = sizeOverLifetime.zMultiplier;
            float a5 = num7;
            float a6 = num8;
            float a7 = num9;
            float num10 = 0f;
            while (num7 != _target)
            {
                num7 = Mathf.Lerp(a5, _target, num10 / timeToBeat);
                num10 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime5 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime5.xMultiplier = num7;
                yield return null;
            }
            while (num8 != _target)
            {
                num8 = Mathf.Lerp(a6, _target, num10 / timeToBeat);
                num10 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime6 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime6.yMultiplier = num8;
                yield return null;
            }
            while (num9 != _target)
            {
                num9 = Mathf.Lerp(a7, _target, num10 / timeToBeat);
                num10 += Time.deltaTime;
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetime7 = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
                sizeOverLifetime7.zMultiplier = num9;
                yield return null;
            }
            m_IsBeat = false;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = base.transform.GetComponent<ParticleSystem>().sizeOverLifetime;
            string a = axis.ToString();
            if (a == "x")
            {
                float num2 = sizeOverLifetime.xMultiplier = Mathf.Lerp(sizeOverLifetime.xMultiplier, min, timeStep * Time.deltaTime);
            }
            if (a == "y")
            {
                float num4 = sizeOverLifetime.yMultiplier = Mathf.Lerp(sizeOverLifetime.yMultiplier, min, timeStep * Time.deltaTime);
            }
            if (a == "z")
            {
                float num6 = sizeOverLifetime.zMultiplier = Mathf.Lerp(sizeOverLifetime.zMultiplier, min, timeStep * Time.deltaTime);
            }
            if (a == "XYZ")
            {
                float xMultiplier = Mathf.Lerp(sizeOverLifetime.xMultiplier, min, timeStep * Time.deltaTime);
                float yMultiplier = Mathf.Lerp(sizeOverLifetime.yMultiplier, min, timeStep * Time.deltaTime);
                float zMultiplier = Mathf.Lerp(sizeOverLifetime.zMultiplier, min, timeStep * Time.deltaTime);
                sizeOverLifetime.xMultiplier = xMultiplier;
                sizeOverLifetime.yMultiplier = yMultiplier;
                sizeOverLifetime.zMultiplier = zMultiplier;
            }
        }
    }


}
