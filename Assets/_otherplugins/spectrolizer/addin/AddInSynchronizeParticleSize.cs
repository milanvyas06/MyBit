// AddInSynchronizeParticleSize
using UnityEngine;

public class AddInSynchronizeParticleSize : MonoBehaviour
{
    [Space]
    public float Beats;

    [Space]
    public Axis _SelectAxis;

    [Space]
    public float beatSize;

    public float minValue;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;


    private void Start()
    {
        AudioSyncwithParticleSize asParticleSize = base.transform.gameObject.AddComponent<AudioSyncwithParticleSize>();
        asParticleSize.bias = Beats;
        asParticleSize.axis = _SelectAxis;
        asParticleSize.max = beatSize;
        asParticleSize.min = minValue;
        asParticleSize._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeParticleSize.Add(asParticleSize);
        }
    }
}
