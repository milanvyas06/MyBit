// AddInSynchronizeRotation
using UnityEngine;

public class AddInSynchronizeRotation : MonoBehaviour
{
    public float Beats;

    [Space]
    public float beatSpeed;

    public float restSpeed;

    [Space]
    private float speed;

    [Space]
    public bool ClockWise;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;

    private void Start()
    {
        AudioSyncWithRotation asRoatation = base.transform.gameObject.AddComponent<AudioSyncWithRotation>();
        asRoatation.bias = Beats;
        asRoatation.max = beatSpeed;
        asRoatation.min = restSpeed;
        asRoatation.isInvertRotate = ClockWise;
        asRoatation._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeRotation.Add(asRoatation);
        }
    }
}
