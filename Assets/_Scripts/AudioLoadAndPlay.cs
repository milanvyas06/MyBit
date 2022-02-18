using System.Collections;
using UnityEngine;

public class AudioLoadAndPlay : MonoBehaviour
{
    public const string name = "Buzz.mp3";

    public AudioSource audioSource;

    public AudioClip audioClip;

    public string fullPath;

    private void Awake()
    {
        audioSource = base.gameObject.AddComponent<AudioSource>();
        fullPath = "file://" + Application.persistentDataPath + "/Sound/";
        StartCoroutine(LoadSong());
    }
    private IEnumerator LoadSong()
    {
        WWW wWW = GetWww(fullPath, "Buzz.mp3");
        yield return wWW;
        audioClip = wWW.GetAudioClip(threeD: false, stream: false);
        audioClip.name = "Buzz.mp3";
        PlaySong();
    }
    private void PlaySong()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = true;
    }
    private WWW GetWww(string fullPath, string fileName)
    {
        string url = string.Format(fullPath + "{0}", fileName);
        return new WWW(url);
    }
}
