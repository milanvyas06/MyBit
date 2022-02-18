// AddInSynchronizeParticle
using UnityEngine;

public class AddInSynchronizeParticle : MonoBehaviour
{
    public float Beats;

    [Space]
    public float beatSimulation;

    public float restSimulation;

    [Space]
    public bool _IsAffectedToUserSetting;

    [Space]
    public ParticleData _ParticleData;
    private void Start()
    {
        AudioSyncWithParticleSimulation asParticleSimulation = base.transform.gameObject.AddComponent<AudioSyncWithParticleSimulation>();
        asParticleSimulation.bias = Beats;
        asParticleSimulation.max = beatSimulation;
        asParticleSimulation.min = restSimulation;
        asParticleSimulation._IsAffectedToUserSetting = _IsAffectedToUserSetting;
        if (_IsAffectedToUserSetting && _ParticleData != null)
        {
            _ParticleData._ParticleLists.Add(asParticleSimulation);
        }
    }

}
