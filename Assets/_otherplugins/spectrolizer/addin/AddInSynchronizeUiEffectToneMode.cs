// AddInSynchronizeUiEffectToneMode
using UnityEngine;

public class AddInSynchronizeUiEffectToneMode : MonoBehaviour
{
    public float Beats;

    [Space]
    public float BeatAmount = 1f;

    public float RestAmount = 0.1f;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithMtone asMTone = base.transform.gameObject.AddComponent<AudioSyncWithMtone>();
        asMTone.bias = Beats;
        asMTone.max = BeatAmount;
        asMTone.min = RestAmount;
        asMTone._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeUiEffectToneMode.Add(asMTone);
        }
    }
}
