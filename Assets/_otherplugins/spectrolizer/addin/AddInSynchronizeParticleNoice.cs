// AddInSynchronizeParticleNoice
using UnityEngine;

public class AddInSynchronizeParticleNoice : MonoBehaviour
{
    public float Beats;

    [Header("This will work if saprate axis is FALSE in noice of particle")]
    [Space]
    public float beatNoice;

    public float restNoice;

    [Space]
    [Header("This will work if saprate axis is TRUE in noice of particle")]
    public Vector3 _beatNoice;

    public Vector3 _restNoice;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithParticleNoice asParticleNoice = base.transform.gameObject.AddComponent<AudioSyncWithParticleNoice>();
        asParticleNoice.bias = Beats;
        asParticleNoice.max = beatNoice;
        asParticleNoice.min = restNoice;
        asParticleNoice._beatNoice = _beatNoice;
        asParticleNoice._restNoice = _restNoice;
        asParticleNoice._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._ParticleNoiseList.Add(asParticleNoice);
        }
    }
}
