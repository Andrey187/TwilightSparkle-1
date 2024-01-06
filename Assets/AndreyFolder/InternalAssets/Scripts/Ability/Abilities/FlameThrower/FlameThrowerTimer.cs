using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FlameThrowerTimer : MonoBehaviour
{
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private float _timerDuration = 30f;
    [SerializeField] private FlameThrowerController _flameThrowerController;
    [Inject] protected IGamePause _gamePause;
    public Button _abilityButton;
    public float FillAmount;

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
    }

    private void ResetTimer()
    {
        _currentTime = 0f;
        UpdateTimerFill();
    }

    private void UpdateTimer()
    {
        if (_currentTime < _timerDuration && !_gamePause.IsPaused)
        {
            _currentTime += Time.deltaTime;
            UpdateTimerFill();
        }
    }

    private void UpdateTimerFill()
    {
        FillAmount = _currentTime / _timerDuration;
        _timerFillImage.fillAmount = FillAmount;
    }

    private void ActivateAbility()
    {
        _flameThrowerController.ActivateFlameThrower();
        ResetTimer();
        _abilityButton.interactable = false;
    }
}
