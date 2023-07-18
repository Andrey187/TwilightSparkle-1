using UnityEngine;

public class AudioSettingsData : MonoBehaviour
{
    private const string BackgroundMusicVolumeKey = "BackgroundMusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string InitialBackgroundMusicVolumeKey = "InitialBackgroundMusicVolumeKey";
    private const string InitialSFXvolumeKey = "InitialSFXvolumeKey";
    public float BackgroundMusicVolume;
    public float SfxVolume;
    public float InitialBackgroundMusicVolume;
    public float InitialSFXvolume;

    public static AudioSettingsData Instanñe;

    private void Awake()
    {
        if (Instanñe == null)
        {
            Instanñe = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitialBackgroundMusicVolume = PlayerPrefs.GetFloat(InitialBackgroundMusicVolumeKey, AudioManager.Instance.MusicSource.volume);
        InitialSFXvolume = PlayerPrefs.GetFloat(InitialSFXvolumeKey, AudioManager.Instance.SfxSource.volume);
        LoadSettings();
    }

    public void LoadSettings()
    {
        // Load the saved slider values from PlayerPrefs
        BackgroundMusicVolume = PlayerPrefs.GetFloat(BackgroundMusicVolumeKey, InitialBackgroundMusicVolume);
        SfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, InitialSFXvolume);
        ApplyMusicSettings();
        ApplySFXSettings();
    }

    public void ApplyMusicSettings()
    {
        AudioManager.Instance.MusicVolume(BackgroundMusicVolume);
    }

    public void ApplySFXSettings()
    {
        AudioManager.Instance.SFXVolume(SfxVolume);
    }

    public void SaveMusicSettings(float value)
    {
        BackgroundMusicVolume = value;

        AudioManager.Instance.MusicVolume(BackgroundMusicVolume);

        PlayerPrefs.SetFloat(BackgroundMusicVolumeKey, Mathf.Clamp(BackgroundMusicVolume,0, InitialBackgroundMusicVolume));
        PlayerPrefs.Save();
    }

    public void SaveSFXSettings(float value)
    {
        SfxVolume = value;

        AudioManager.Instance.SFXVolume(SfxVolume);

        PlayerPrefs.SetFloat(SFXVolumeKey, Mathf.Clamp(SfxVolume, 0, InitialSFXvolume));
        PlayerPrefs.Save();
    }
}
