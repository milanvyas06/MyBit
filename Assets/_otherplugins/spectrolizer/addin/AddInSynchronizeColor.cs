// AddInSynchronizeColor
using UnityEngine;
using UnityEngine.UI;

public class AddInSynchronizeColor : MonoBehaviour
{
    [Space]
    [Header("Text Color")]
    [Header("Image Color")]
    [Header("Use this script to modify:")]
    [Header("Light Color")]
    [Header("Particle Color")]
    public float Beats;

    [Space]
    public bool UseWithoutSelf;

    [Space]
    public Color[] beatColors;

    public Color restColor;

    public Image[] _ImageList;

    public Text[] _TextList;

    public ParticleSystem[] _ParticleSystem;

    public Light[] _Lights;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;

    private void Start()
    {
        AudioSyncWithColor asWithColor = base.transform.gameObject.AddComponent<AudioSyncWithColor>();
        asWithColor.bias = Beats;
        asWithColor.shouldChangeParticleMatColor = UseWithoutSelf;
        asWithColor.colors = beatColors;
        asWithColor.defaultColor = restColor;
        asWithColor._ImageList = _ImageList;
        asWithColor._TextList = _TextList;
        asWithColor._ParticleSystem = _ParticleSystem;
        asWithColor._Lights = _Lights;
        asWithColor._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeColor.Add(asWithColor);
        }
    }

}
