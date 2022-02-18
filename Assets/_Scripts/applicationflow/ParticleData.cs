using System.Collections.Generic;
using UnityEngine;

public class ParticleData : MonoBehaviour
{
    public static ParticleData instance;

    [Space]
    public WaterMarkPosition _WaterMarkPosition;

    [Space]
    public List<AudioSyncWithParticleSimulation> _ParticleLists;

    [Space]
    public List<AudioSyncWithParticleNoice> _ParticleNoiseList;

    [Space]
    public List<AudioSyncWithColor> _SynchronizeColor;

    [Space]
    public List<AudioSyncWithImageFillAmount> _SynchronizeImageFill;

    [Space]
    public List<AudioSyncWithLight> _SynchronizeLight;

    [Space]
    public List<AudioSyncWithGivenAxis> _SynchronizeScales;

    [Space]
    public List<AudioSyncWithRandomLocalPos> _SynchronizeVibration;

    [Space]
    public List<AudioSyncWithLensFlareBright> _SynchroniseFlare;

    [Space]
    public List<AudioSyncWithMtone> _SynchronizeUiEffectToneMode;

    [Space]
    public List<AudioSyncWithGivenTone> _SynchronizeGrayscaleToneMode;

    [Space]
    public List<AudioSyncWithRotation> _SynchronizeRotation;

    [Space]
    public List<AudioSyncWithGivenPos> _SynchronizePosition;

    [Space]
    public List<AudioSyncwithParticleSize> _SynchronizeParticleSize;

    [Space]
    public List<AudioSyncWithGradiant> _SynchronizeUIGradient;

    [Space]
    public UserTextList[] _Texts;

    [Space]
    public Camera[] _Camera;


    private void Awake()
    {
        instance = this;
    }

}
