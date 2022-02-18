using UnityEngine;

public class AudioSpecturmBase : MonoBehaviour
{
    public float bias = 10f;

    [HideInInspector]
    public float timeStep = 0.005f;

    [HideInInspector]
    public float timeToBeat = 0.025f;

    [HideInInspector]
    public float TotalTimeT = 7f;

    private float m_PreviousAudioValue;

    private float m_AudioValue;

    private float m_Timer;

    protected bool m_IsBeat;

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnStart()
    {
    }
    public virtual void OnBeat()
    {
        m_Timer = 0f;
        m_IsBeat = true;
    }
    public virtual void OnUpdate()
    {
        m_PreviousAudioValue = m_AudioValue;
        m_AudioValue = AudioSpectrum.spectrumValue;
        if (m_PreviousAudioValue > bias && m_AudioValue <= bias && m_Timer > timeStep)
        {
            OnBeat();
        }
        if (m_PreviousAudioValue <= bias && m_AudioValue > bias && m_Timer > timeStep)
        {
            OnBeat();
        }
        m_Timer += Time.deltaTime;
    }



}
