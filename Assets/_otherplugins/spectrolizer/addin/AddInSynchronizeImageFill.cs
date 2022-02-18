// AddInSynchronizeImageFill
using UnityEngine;

public class AddInSynchronizeImageFill : MonoBehaviour
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
        AudioSyncWithImageFillAmount asImageFillAmount = base.transform.gameObject.AddComponent<AudioSyncWithImageFillAmount>();
        asImageFillAmount.bias = Beats;
        asImageFillAmount.max = BeatAmount;
        asImageFillAmount.min = RestAmount;
        asImageFillAmount._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeImageFill.Add(asImageFillAmount);
        }
    }
}
