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

    public void PlaySFX(string name)
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
        sfxSource.loop = sound.loop;
        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Music '" + name + "' not found");
            return;
        }

        musicSource.clip = sound.clip;
        musicSource.volume = sound.volume;
        musicSource.pitch = sound.pitch;
        musicSource.loop = sound.loop;
        musicSource.Play();
    }

    [ContextMenu("Sort Sounds By Name")]
    void DoSortSounds()
    {
        System.Array.Sort(sounds, (a, b) => a.name.CompareTo(b.name));
    }
}
