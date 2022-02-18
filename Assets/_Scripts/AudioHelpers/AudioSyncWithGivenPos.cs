using System.Collections;
using UnityEngine;

public class AudioSyncWithGivenPos : AudioSpecturmBase
{
    public float max;

    public float min;

    [Space]
    public bool _IsAffectedToUserSetting;

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("MoveToPosition");
        StartCoroutine("MoveToPosition", max);
    }

    private IEnumerator MoveToPosition(float _target)
    {
        Vector3 localPosition = base.transform.localPosition;
        float num = localPosition.y;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            Transform transform = base.transform;
            Vector3 localPosition2 = base.transform.localPosition;
            float x = localPosition2.x;
            float y = num;
            Vector3 localPosition3 = base.transform.localPosition;
            transform.localPosition = new Vector3(x, y, localPosition3.z);
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            Vector3 localPosition = base.transform.localPosition;
            float y = Mathf.Lerp(localPosition.y, min, TotalTimeT * Time.deltaTime);
            Transform transform = base.transform;
            Vector3 localPosition2 = base.transform.localPosition;
            float x = localPosition2.x;
            Vector3 localPosition3 = base.transform.localPosition;
            transform.localPosition = new Vector3(x, y, localPosition3.z);
        }
    }

}
