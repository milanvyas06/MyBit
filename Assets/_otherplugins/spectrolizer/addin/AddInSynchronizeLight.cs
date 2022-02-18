// AddInSynchronizeLight
using UnityEngine;

public class AddInSynchronizeLight : MonoBehaviour
{
    public float Beats;

    [Space]
    public float beatIntensity;

    public float restIntensity;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;

    private void Start()
    {
        AudioSyncWithLight asWithLight = base.transform.gameObject.AddComponent<AudioSyncWithLight>();
        asWithLight.bias = Beats;
        asWithLight.max = beatIntensity;
        asWithLight.min = restIntensity;
        asWithLight._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeLight.Add(asWithLight);
        }
    }

}
