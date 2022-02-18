// AddInSynchronizeVibration
using UnityEngine;

public class AddInSynchronizeVibration : MonoBehaviour
{
    public float Beat;

    [Space]
    public float beatVibration = 10f;

    public float restVibration;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithRandomLocalPos asVibration = base.transform.gameObject.AddComponent<AudioSyncWithRandomLocalPos>();
        asVibration.max = beatVibration;
        asVibration.min = restVibration;
        asVibration._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeVibration.Add(asVibration);
        }
    }

}
