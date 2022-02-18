using System.Collections;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class AudioSyncWithGivenTone : AudioSpecturmBase
{
    [Space]
    public float max = 1f;

    public float min = 0.1f;

    [Space]
    public bool _IsAffectedToUserSetting;

    Image img;
    Material mat;

    GrayscaleEffectApplier grayscale;

    private void Start()
    {
        img = GetComponent<Image>();
        mat = img.material;

        grayscale = GetComponent<GrayscaleEffectApplier>();
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
            grayscale.effect = Mathf.Lerp(grayscale.effect, min, TotalTimeT * Time.deltaTime);
        }
    }

    private IEnumerator UIEffectToneLevel(float _target)
    {
        float num = grayscale.effect;
        float a = num;
        float num2 = 0f;
        while (num != _target)
        {
            num = Mathf.Lerp(a, _target, num2 / timeToBeat);
            num2 += Time.deltaTime;
            grayscale.effect = num;
            yield return null;
        }
        m_IsBeat = false;
    }

}
