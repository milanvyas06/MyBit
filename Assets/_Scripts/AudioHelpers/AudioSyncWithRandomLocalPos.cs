using System.Collections;
using UnityEngine;

public class AudioSyncWithRandomLocalPos : AudioSpecturmBase
{
    private float scaleQty;

    private Vector3 currPos;

    [Space]
    public float max = 10f;

    public float min;

    [Space]
    public bool _IsAffectedToUserSetting;



    public override void OnStart()
    {
        base.OnStart();
        currPos = base.transform.localPosition;
    }
    public override void OnUpdate()
    {
        Vector3 vector = currPos + Random.insideUnitSphere * scaleQty;
        Transform transform = base.transform;
        float x = vector.x;
        float y = vector.y;
        Vector3 localPosition = base.transform.localPosition;
        transform.localPosition = new Vector3(x, y, localPosition.z);
        base.OnUpdate();
        if (!m_IsBeat)
        {
            scaleQty = Mathf.Lerp(scaleQty, min, TotalTimeT * Time.deltaTime);
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("ObjectVibration");
        StartCoroutine("ObjectVibration", max);
    }

    private IEnumerator ObjectVibration(float _target)
    {
        float num = scaleQty;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            scaleQty = num;
            yield return null;
        }
        m_IsBeat = false;
    }

}
