using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] MusicSounds, SfxSounds;
    public AudioSource MusicSource, SfxSource;

    private float _soundCooldown = 0.01f;
    private float _lastSoundTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void PlayMusic(Sound.SoundEnum name)
    {
        Sound s = Array.Find(MusicSounds, x => x.SoundType == name);

        if(s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            MusicSource.clip = s.Clip;
            MusicSource.Play();
        }
    }

    public void PlaySFX(Sound.SoundEnum name)
    {
        if (Time.time - _lastSoundTime >= _soundCooldown)
        {
            Sound s = Array.Find(SfxSounds, x => x.SoundType == name);

            if (s == null)
            {
                Debug.Log("Sfx not found");
            }
            else
            {
                SfxSource.PlayOneShot(s.Clip);
                _lastSoundTime = Time.time;
            }
        }
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SfxSource.volume = volume;
    }

    public void FadeOutMusic(float fadeProgress)
    {
        MusicSource.volume *= fadeProgress;
    }
}
