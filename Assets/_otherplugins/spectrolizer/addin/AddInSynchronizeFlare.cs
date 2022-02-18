// AddInSynchronizeFlare
using UnityEngine;

public class AddInSynchronizeFlare : MonoBehaviour
{
    public float Beats;

    [Space]
    public float beatBrightness = 10f;

    public float restBrightness;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;

    private void Start()
    {
        AudioSyncWithLensFlareBright asLensFlare = base.transform.gameObject.AddComponent<AudioSyncWithLensFlareBright>();
        asLensFlare.bias = Beats;
        asLensFlare.max = beatBrightness;
        asLensFlare.min = restBrightness;
        asLensFlare._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchroniseFlare.Add(asLensFlare);
        }
    }
}
