// AddInSynchronizeUiEffectToneMode
using UnityEngine;

public class AddInSynchronizeGivenGrayscale : MonoBehaviour
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
        AudioSyncWithGivenTone asMTone = base.transform.gameObject.AddComponent<AudioSyncWithGivenTone>();
        asMTone.bias = Beats;
        asMTone.max = BeatAmount;
        asMTone.min = RestAmount;
        asMTone._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeGrayscaleToneMode.Add(asMTone);
        }
    }
}
