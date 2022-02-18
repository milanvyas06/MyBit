using System;
using System.Collections;
using UnityEngine;


public class AudioSyncWithGivenAxis : AudioSpecturmBase
{
    [Space]
    public Vector3 max = new Vector3(1.01f, 1.01f, 1.01f);

    public Vector3 min = new Vector3(1f, 1f, 1f);

    [Space]
    public Axiss axiss;

    [Space]
    public bool _IsAffectedToUserSetting;

    private IEnumerator MoveToScale(Vector3 _target)
    {
        Vector3 vector = base.transform.localScale;
        Vector3 a = vector;
        float num = 0f;
        while (vector != _target)
        {
            vector = Vector3.Lerp(a, _target, num / timeToBeat);
            num += Time.deltaTime;
            string text = axiss.ToString();
            if (text.Equals("All"))
            {
                base.transform.localScale = vector;
            }
            if (text.Equals("x"))
            {
                base.transform.localScale = new Vector3(vector.x, min.y, min.z);
            }
            if (text.Equals("y"))
            {
                base.transform.localScale = new Vector3(min.x, vector.y, min.z);
            }
            if (text.Equals("z"))
            {
                base.transform.localScale = new Vector3(min.x, min.y, vector.z);
            }
            yield return null;
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_IsBeat)
        {
            base.transform.localScale = Vector3.Lerp(base.transform.localScale, min, TotalTimeT * Time.deltaTime);
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", max);
    }

}
