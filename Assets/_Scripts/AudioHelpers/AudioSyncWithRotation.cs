using System.Collections;
using UnityEngine;

public class AudioSyncWithRotation : AudioSpecturmBase
{
    [Space]
    public float max;

    public float min;

    [Space]
    public float defaultPos;

    [Space]
    public bool isInvertRotate;

    [Space]
    public bool _IsAffectedToUserSetting;

    public override void OnUpdate()
    {
        if (isInvertRotate)
        {
            base.transform.Rotate(0f, 0f, (0f - Time.deltaTime) * defaultPos);
        }
        else
        {
            base.transform.Rotate(0f, 0f, Time.deltaTime * defaultPos);
        }
        base.OnUpdate();
        if (!m_IsBeat)
        {
            float num = defaultPos = Mathf.Lerp(defaultPos, min, TotalTimeT * Time.deltaTime);
        }
    }

    private IEnumerator RotationSpeed(float _target)
    {
        float num = defaultPos;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            defaultPos = num;
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("RotationSpeed");
        StartCoroutine("RotationSpeed", max);
    }

}
