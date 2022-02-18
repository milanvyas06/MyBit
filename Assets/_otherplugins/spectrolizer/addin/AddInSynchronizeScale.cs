// AddInSynchronizeScale
using UnityEngine;

public class AddInSynchronizeScale : MonoBehaviour
{

    [Space]
    public float Beats;

    [Space]
    public Vector3 beatScale = new Vector3(1.01f, 1.01f, 1.01f);

    public Vector3 restScale = new Vector3(1f, 1f, 1f);

    [Space]
    public Axiss _ScaleOnAxis;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithGivenAxis asGivenScale = base.transform.gameObject.AddComponent<AudioSyncWithGivenAxis>();
        asGivenScale.bias = Beats;
        asGivenScale.max = beatScale;
        asGivenScale.min = restScale;
        asGivenScale.axiss = _ScaleOnAxis;
        asGivenScale._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._SynchronizeScales.Add(asGivenScale);
        }
    }

}
