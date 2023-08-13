using UnityEngine;
using UnityEngine.UI;

public class ShieldWithTimer : MonoBehaviour
{
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private Button _abilityButton;
    [SerializeField] private MagicShield _magicShield;

    [SerializeField] private float _timerDuration = 30f;
    private float _currentTime;

    private void Start()
    {
        _abilityButton.interactable = false;
        _abilityButton.onClick.AddListener(ActivateAbility);
        ResetTimer();
    }

    private void Update()
    {
        UpdateTimer();
        UpdateButtonInteractability();
    }

    private void ResetTimer()
    {
        _currentTime = 0f;
        UpdateTimerFill();
    }

    private void UpdateTimer()
    {
        if (_currentTime < _timerDuration)
        {
            _currentTime += Time.deltaTime;
            UpdateTimerFill();
        }
    }

    private void UpdateTimerFill()
    {
        float fillAmount = _currentTime / _timerDuration;
        _timerFillImage.fillAmount = fillAmount;
    }

    private void UpdateButtonInteractability()
    {
        _abilityButton.interactable = _timerFillImage.fillAmount >= 1f;
    }

    private void ActivateAbility()
    {
        _magicShield.ActivateShield();
        ResetTimer();
    }
}
