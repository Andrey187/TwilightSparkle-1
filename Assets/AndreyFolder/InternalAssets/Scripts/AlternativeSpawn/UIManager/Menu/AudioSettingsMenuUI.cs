using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenuUI : MonoBehaviour
{
    public Slider BackgroundMusicSlider;
    public Slider SFXSlider;

    private AudioSettingsData _audioSettingsData;
    private void Awake()
    {
        _audioSettingsData = FindObjectOfType<AudioSettingsData>();

        BackgroundMusicSlider.minValue = 0f;
        BackgroundMusicSlider.maxValue = _audioSettingsData.InitialBackgroundMusicVolume;

        SFXSlider.minValue = 0f;
        SFXSlider.maxValue = _audioSettingsData.InitialSFXvolume;

        BackgroundMusicSlider.onValueChanged.AddListener(OnBackgroundMusicSliderChanged);
        SFXSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void Start()
    {
        LoadSettings();
    }
    private void LoadSettings()
    {
        BackgroundMusicSlider.value = _audioSettingsData.BackgroundMusicVolume;
        SFXSlider.value = _audioSettingsData.SfxVolume;
    }

    public void OnBackgroundMusicSliderChanged(float value)
    {
        _audioSettingsData.BackgroundMusicVolume = value;
        _audioSettingsData.ApplyMusicSettings();
        _audioSettingsData.SaveMusicSettings(value);
    }

    public void OnSFXSliderChanged(float value)
    {
        _audioSettingsData.SfxVolume = value;
        _audioSettingsData.ApplySFXSettings();
        _audioSettingsData.SaveSFXSettings(value);
    }

    public void OnResumeButtonClicked()
    {
        SettingsMenuManager settingsMenuManager = GetComponentInParent<SettingsMenuManager>();
        settingsMenuManager.HideMusicSettingsMenu();
    }
}
