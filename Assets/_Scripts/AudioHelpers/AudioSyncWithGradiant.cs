using Coffee.UIExtensions;
using System.Collections;
using UnityEngine;

public class AudioSyncWithGradiant : AudioSpecturmBase
{
    [Space]
    public bool IsSynchronizeOffset;

    public bool IsSynchronizeVerticleHorizontalOffset;

    public bool IsSynchronizeRotation;

    public bool IsSynchronizeTwoColor;

    public bool IsSynchronizeFourColor;

    [Space]
    public float maxGradiant;

    public float minGradiant;

    [Space]
    public Vector2 maxVhOffset;

    public Vector2 minVgOffset;

    [Space]
    public RotationType rotatinType;

    public float gradiantRotateMax;

    public float gradiantRotateMin;

    private float current;

    private float rotaionVal = -180f;

    [Space]
    public bool _IsAffectedToUserSetting;

    private IEnumerator UIGradientOffset(float _target)
    {
        if (IsSynchronizeOffset)
        {
            float num = base.transform.GetComponent<UIGradient>().offset;
            float a = num;
            float num2 = 0f;
            while (num != _target)
            {
                num = Mathf.Lerp(a, _target, num2 / timeToBeat);
                num2 += Time.deltaTime;
                base.transform.GetComponent<UIGradient>().offset = num;
                yield return null;
            }
        }
        m_IsBeat = false;
    }

    private IEnumerator UIGradientRotation(float _target)
    {
        if (IsSynchronizeRotation)
        {
            string a = rotatinType.ToString();
            if (a == "BySpeed")
            {
                float num = current;
                float a2 = num;
                float num2 = 0f;
                while (num != _target)
                {
                    num = Mathf.Lerp(a2, _target, num2 / timeToBeat);
                    num2 += Time.deltaTime;
                    current = num;
                    yield return null;
                }
            }
            else
            {
                float num3 = base.transform.GetComponent<UIGradient>().rotation;
                float a3 = num3;
                float num4 = 0f;
                while (num3 != _target)
                {
                    num3 = Mathf.Lerp(a3, _target, num4 / timeToBeat);
                    num4 += Time.deltaTime;
                    base.transform.GetComponent<UIGradient>().rotation = num3;
                    yield return null;
                }
            }
        }
        m_IsBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        if (IsSynchronizeOffset)
        {
            StopCoroutine("UIGradientOffset");
            StartCoroutine("UIGradientOffset", maxGradiant);
        }
        if (IsSynchronizeVerticleHorizontalOffset)
        {
            StopCoroutine("UIGradientVerticleAndHorizontalOffset");
            StartCoroutine("UIGradientVerticleAndHorizontalOffset", maxVhOffset);
        }
        if (IsSynchronizeRotation)
        {
            string a = rotatinType.ToString();
            if (a == "BySpeed")
            {
                StopCoroutine("UIGradientRotation");
                StartCoroutine("UIGradientRotation", gradiantRotateMax);
            }
            else
            {
                StopCoroutine("UIGradientRotation");
                StartCoroutine("UIGradientRotation", gradiantRotateMax);
            }
        }
    }

    private IEnumerator UIGradientVerticleAndHorizontalOffset(Vector2 _target)
    {
        if (IsSynchronizeVerticleHorizontalOffset)
        {
            Vector2 offset = base.transform.GetComponent<UIGradient>().offset2;
            float x = offset.x;
            Vector2 offset2 = base.transform.GetComponent<UIGradient>().offset2;
            Vector2 lhs = new Vector2(x, offset2.y);
            Vector2 a = new Vector2(lhs.x, lhs.y);
            float num = 0f;
            while (lhs != _target)
            {
                lhs = Vector2.Lerp(a, _target, num / timeToBeat);
                num += Time.deltaTime;
                base.transform.GetComponent<UIGradient>().offset2 = new Vector2(lhs.x, lhs.y);
                yield return null;
            }
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        string a = rotatinType.ToString();
        if (IsSynchronizeRotation && a == "BySpeed")
        {
            if (base.transform.GetComponent<UIGradient>().rotation > 180f)
            {
                base.transform.GetComponent<UIGradient>().rotation = rotaionVal;
            }
            base.transform.GetComponent<UIGradient>().rotation = base.transform.GetComponent<UIGradient>().rotation + Time.deltaTime * current;
        }
        base.OnUpdate();
        if (m_IsBeat)
        {
            return;
        }
        if (IsSynchronizeOffset)
        {
            base.transform.GetComponent<UIGradient>().offset = Mathf.Lerp(base.transform.GetComponent<UIGradient>().offset, minGradiant, TotalTimeT * Time.deltaTime);
        }
        if (IsSynchronizeVerticleHorizontalOffset)
        {
            base.transform.GetComponent<UIGradient>().offset2 = Vector2.Lerp(base.transform.GetComponent<UIGradient>().offset2, minVgOffset, TotalTimeT * Time.deltaTime);
        }
        if (IsSynchronizeRotation)
        {
            if (a == "BySpeed")
            {
                current = Mathf.Lerp(current, gradiantRotateMin, TotalTimeT * Time.deltaTime);
            }
            else
            {
                base.transform.GetComponent<UIGradient>().rotation = Mathf.Lerp(base.transform.GetComponent<UIGradient>().rotation, gradiantRotateMin, TotalTimeT * Time.deltaTime);
            }
        }
    }

}
