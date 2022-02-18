// AddInSynchronizePosition
using UnityEngine;

public class AddInSynchronizePosition : MonoBehaviour
{
    public float Beats;

    [Space]
    public float beatPos;

    public float restPos;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithGivenPos asWithPosition = base.transform.gameObject.AddComponent<AudioSyncWithGivenPos>();
        asWithPosition.bias = Beats;
        asWithPosition.max = beatPos;
        asWithPosition.min = restPos;
        asWithPosition._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizePosition.Add(asWithPosition);
        }
    }

}
