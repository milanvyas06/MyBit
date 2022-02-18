using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSyncWithColor : AudioSpecturmBase
{
    [Space]
    public bool shouldChangeParticleMatColor;

    [Space]
    public Color[] colors;

    public Color defaultColor;

    private int num;

    private Image img;

    private Text txt;

    private ParticleSystem particle;

    private Material mat;

    private Light light;

    private List<Material> _MaterialList = new List<Material>();

    public Image[] _ImageList;

    public Text[] _TextList;

    public ParticleSystem[] _ParticleSystem;

    public Light[] _Lights;

    [Space]
    public bool _IsAffectedToUserSetting;

    private Color GetColor()
    {
        if (colors == null || colors.Length == 0)
        {
            return Color.white;
        }
        num = Random.Range(0, colors.Length);
        return colors[num];
    }



    public override void OnBeat()
    {
        base.OnBeat();
        Color color = GetColor();
        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", color);
    }

    private void Start()
    {
        if (base.transform.GetComponent<Image>() != null)
        {
            img = GetComponent<Image>();
        }
        else if (base.transform.GetComponent<Text>() != null)
        {
            txt = GetComponent<Text>();
        }
        else if (base.transform.GetComponent<ParticleSystem>() != null)
        {
            particle = GetComponent<ParticleSystem>();
            ParticleSystemRenderer component = GetComponent<ParticleSystemRenderer>();
            mat = component.material;
        }
        else if (base.transform.GetComponent<Light>() != null)
        {
            light = base.transform.GetComponent<Light>();
        }
        if (shouldChangeParticleMatColor && _ParticleSystem.Length > 0)
        {
            for (int i = 0; i < _ParticleSystem.Length; i++)
            {
                ParticleSystemRenderer component2 = _ParticleSystem[i].gameObject.transform.GetComponent<ParticleSystemRenderer>();
                _MaterialList.Add(component2.material);
            }
        }
    }

    private IEnumerator MoveToColor(Color _target)
    {
        if (!shouldChangeParticleMatColor)
        {
            if (base.transform.GetComponent<Image>() != null)
            {
                Color color = img.color;
                Color a = color;
                float num = 0f;
                while (color != _target)
                {
                    color = Color.Lerp(a, _target, num / timeToBeat);
                    num += Time.deltaTime;
                    img.color = color;
                    yield return null;
                }
            }
            else if (base.transform.GetComponent<Text>() != null)
            {
                Color color2 = txt.color;
                Color a2 = color2;
                float num2 = 0f;
                while (color2 != _target)
                {
                    color2 = Color.Lerp(a2, _target, num2 / timeToBeat);
                    num2 += Time.deltaTime;
                    txt.color = color2;
                    yield return null;
                }
            }
            else if (base.transform.GetComponent<ParticleSystem>() != null)
            {
                Color color3 = mat.GetColor("_TintColor");
                Color a3 = color3;
                float num3 = 0f;
                while (color3 != _target)
                {
                    color3 = Color.Lerp(a3, _target, num3 / timeToBeat);
                    num3 += Time.deltaTime;
                    mat.SetColor("_TintColor", color3);
                    Renderer component = GetComponent<ParticleSystemRenderer>();
                    yield return null;
                }
            }
            else if (base.transform.GetComponent<Light>() != null)
            {
                Color color4 = light.color;
                Color a4 = color4;
                float num4 = 0f;
                while (color4 != _target)
                {
                    color4 = Color.Lerp(a4, _target, num4 / timeToBeat);
                    num4 += Time.deltaTime;
                    light.color = color4;
                }
            }
            m_IsBeat = false;
            yield break;
        }
        if (_ImageList.Length > 0)
        {
            for (int i = 0; i < _ImageList.Length; i++)
            {
                Color color5 = _ImageList[i].color;
                Color a5 = color5;
                float num5 = 0f;
                while (color5 != _target)
                {
                    color5 = Color.Lerp(a5, _target, num5 / timeToBeat);
                    num5 += Time.deltaTime;
                    _ImageList[i].color = color5;
                }
            }
        }
        if (_TextList.Length > 0)
        {
            for (int j = 0; j < _TextList.Length; j++)
            {
                Color color6 = _TextList[j].color;
                Color a6 = color6;
                float num6 = 0f;
                while (color6 != _target)
                {
                    color6 = Color.Lerp(a6, _target, num6 / timeToBeat);
                    num6 += Time.deltaTime;
                    _TextList[j].color = color6;
                }
            }
        }
        if (_ParticleSystem.Length > 0)
        {
            for (int k = 0; k < _ParticleSystem.Length; k++)
            {
                Color color7 = _MaterialList[k].GetColor("_TintColor");
                Color a7 = color7;
                float num7 = 0f;
                while (color7 != _target)
                {
                    color7 = Color.Lerp(a7, _target, num7 / timeToBeat);
                    num7 += Time.deltaTime;
                    _MaterialList[k].SetColor("_TintColor", color7);
                    yield return null;
                }
            }
        }
        if (_Lights.Length > 0)
        {
            for (int l = 0; l < _Lights.Length; l++)
            {
                Color color8 = _Lights[l].color;
                Color a8 = color8;
                float num8 = 0f;
                while (color8 != _target)
                {
                    color8 = Color.Lerp(a8, _target, num8 / timeToBeat);
                    num8 += Time.deltaTime;
                    _Lights[l].color = color8;
                }
            }
        }
        m_IsBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (m_IsBeat)
        {
            return;
        }
        if (!shouldChangeParticleMatColor)
        {
            if (base.transform.GetComponent<Image>() != null)
            {
                img.color = Color.Lerp(img.color, defaultColor, TotalTimeT * Time.deltaTime);
            }
            else if (base.transform.GetComponent<Text>() != null)
            {
                txt.color = Color.Lerp(txt.color, defaultColor, TotalTimeT * Time.deltaTime);
            }
            else if (base.transform.GetComponent<ParticleSystem>() != null)
            {
                mat.SetColor("_TintColor", Color.Lerp(mat.GetColor("_TintColor"), defaultColor, TotalTimeT * Time.deltaTime));
                Renderer component = GetComponent<ParticleSystemRenderer>();
            }
            else if (base.transform.GetComponent<Light>() != null)
            {
                light.color = Color.Lerp(light.color, defaultColor, TotalTimeT * Time.deltaTime);
            }
            return;
        }
        if (_ImageList.Length > 0)
        {
            for (int i = 0; i < _ImageList.Length; i++)
            {
                _ImageList[i].color = Color.Lerp(_ImageList[i].color, defaultColor, TotalTimeT * Time.deltaTime);
            }
        }
        if (_TextList.Length > 0)
        {
            for (int j = 0; j < _TextList.Length; j++)
            {
                _TextList[j].color = Color.Lerp(_TextList[j].color, defaultColor, TotalTimeT * Time.deltaTime);
            }
        }
        if (_ParticleSystem.Length > 0)
        {
            for (int k = 0; k < _ParticleSystem.Length; k++)
            {
                _MaterialList[k].SetColor("_TintColor", Color.Lerp(_MaterialList[k].GetColor("_TintColor"), defaultColor, TotalTimeT * Time.deltaTime));
            }
        }
        if (_Lights.Length > 0)
        {
            for (int l = 0; l < _Lights.Length; l++)
            {
                _Lights[l].color = Color.Lerp(_Lights[l].color, defaultColor, TotalTimeT * Time.deltaTime);
            }
        }
    }


}
