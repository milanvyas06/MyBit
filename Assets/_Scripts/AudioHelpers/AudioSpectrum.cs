using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    private float[] m_AudioSpectrum;

    public float specturmMultipler;

    public static float spectrumValue
    {
        get;
        private set;
    }

    private void Update()
    {
        AudioListener.GetSpectrumData(m_AudioSpectrum, 0, FFTWindow.Hamming);
        if (m_AudioSpectrum != null && m_AudioSpectrum.Length > 0)
        {
            spectrumValue = m_AudioSpectrum[0] * specturmMultipler;
        }
    }

    private void Start()
    {
        m_AudioSpectrum = new float[128];

    }

}
