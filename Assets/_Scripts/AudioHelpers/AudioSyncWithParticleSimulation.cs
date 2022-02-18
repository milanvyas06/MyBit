using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AudioSyncWithParticleSimulation : AudioSpecturmBase
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
            ParticleSystem.MainModule main = base.transform.GetComponent<ParticleSystem>().main;
            float num2 = main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, min, TotalTimeT * Time.deltaTime);
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("ParticleSimmulation");
        StartCoroutine("ParticleSimmulation", max);
    }

    private IEnumerator ParticleSimmulation(float _target)
    {
        float num = base.transform.GetComponent<ParticleSystem>().main.simulationSpeed;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            ParticleSystem.MainModule main = base.transform.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = num;
            yield return null;
        }
        m_IsBeat = false;
    }

}
