using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public Sound[] sounds;

    void Awake()
    {
        if (audioManagerInstance == null)
        {
            audioManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found");
            return;
        }

        sfxSource.clip = sound.clip;
        sfxSource.volume = sound.volume;
        sfxSource.pitch = sound.pitch;
        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }
}
