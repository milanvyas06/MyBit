using Coffee.UIExtensions;
using System.Collections;
using UnityEngine;

public class AudioSyncWithMtone : AudioSpecturmBase
{
    [Space]
    public float max = 1f;

    public float min = 0.1f;

    [Space]
    public bool _IsAffectedToUserSetting;

    private IEnumerator UIEffectToneLevel(float _target)
    {
        float num = base.transform.GetComponent<UIEffect>().effectFactor;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            base.transform.GetComponent<UIEffect>().effectFactor = num;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("UIEffectToneLevel");
        StartCoroutine("UIEffectToneLevel", max);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            base.transform.GetComponent<UIEffect>().effectFactor = Mathf.Lerp(base.transform.GetComponent<UIEffect>().effectFactor, min, TotalTimeT * Time.deltaTime);
        }
    }

}
