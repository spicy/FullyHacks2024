using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;
    public float volume = 1f;
    public float pitch = 1f;
    public float volumeDeviation = 0.1f;
    public float pitchDeviation = 0.1f;

    [HideInInspector]
    public AudioSource source;

    public void Initialize(GameObject parent, AudioMixerGroup audioMxrGroup)
    {
        source = parent.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.outputAudioMixerGroup = audioMxrGroup;
    }

    public void Play()
    {
        float volumeVariance = Random.Range(1 - volumeDeviation / 2, 1 + volumeDeviation / 2);
        float pitchVariance = Random.Range(1 - pitchDeviation / 2, 1 + pitchDeviation / 2);
        source.volume = volume * volumeVariance;
        source.pitch = pitch * pitchVariance;
        source.Play();
    }

    public void FadeOut(float duration)
    {
        source.volume -= volume * Time.deltaTime / duration;
    }
}