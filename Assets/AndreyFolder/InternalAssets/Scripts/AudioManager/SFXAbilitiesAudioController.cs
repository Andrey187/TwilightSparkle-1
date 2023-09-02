using UnityEngine;
using Zenject;

public class SFXAbilitiesAudioController : MonoBehaviour
{
    [Inject] private IAbilitySpawn abilitySpawn;
    private AudioManager _audioManager;
    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        abilitySpawn.PlaySound += _audioManager.PlaySFX;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        abilitySpawn.PlaySound -= _audioManager.PlaySFX;
    }
}
