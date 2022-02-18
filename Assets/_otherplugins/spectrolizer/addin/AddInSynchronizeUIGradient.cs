// AddInSynchronizeUIGradient
using UnityEngine;

public class AddInSynchronizeUIGradient : MonoBehaviour
{
    public float Beats;

    [Space]
    public bool IsSynchronizeOffset;

    public bool IsSynchronizeVerticleHorizontalOffset;

    public bool IsSynchronizeRotation;

    public bool IsSynchronizeTwoColor;

    public bool IsSynchronizeFourColor;

    [Header("This will be synchronize offset, if IsSynchronizeOffset is true")]
    [Space]
    public float beatOffset;

    public float restOffset;

    [Header("This will be synchronize offset Verticle And Horizontal, if IsSynchronizeVerticleHorizontalOffset is true")]
    [Space]
    public Vector2 VAndHBeatOffset;

    public Vector2 VAndHRestOffset;

    [Header("This will be synchronize rotation, if Is Synchronize Rotation is true")]
    [Space]
    public RotationType Type;

    public float beatRotation;

    public float restRotation;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;


    private void Start()
    {
        AudioSyncWithGradiant asGradiant = base.transform.gameObject.AddComponent<AudioSyncWithGradiant>();
        asGradiant.bias = Beats;
        asGradiant.IsSynchronizeOffset = IsSynchronizeOffset;
        asGradiant.IsSynchronizeVerticleHorizontalOffset = IsSynchronizeVerticleHorizontalOffset;
        asGradiant.IsSynchronizeRotation = IsSynchronizeRotation;
        asGradiant.IsSynchronizeTwoColor = IsSynchronizeTwoColor;
        asGradiant.IsSynchronizeFourColor = IsSynchronizeFourColor;
        asGradiant.maxGradiant = beatOffset;
        asGradiant.minGradiant = restOffset;
        asGradiant.maxVhOffset = VAndHBeatOffset;
        asGradiant.minVgOffset = VAndHRestOffset;
        asGradiant.rotatinType = Type;
        asGradiant.gradiantRotateMax = beatRotation;
        asGradiant.gradiantRotateMin = restRotation;
        asGradiant._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeUIGradient.Add(asGradiant);
        }
    }


}
