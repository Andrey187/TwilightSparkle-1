using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIPanelController : MonoCache
{
    [SerializeField] private GameObject _restartGamePanel;
    [SerializeField] private GameObject _abilitySelectionPanel;
    [SerializeField] private GameObject _talentTreePanel;
    [SerializeField] private GameObject _joystickPanel;
    [SerializeField] private GameObject _allUi;
    [SerializeField] private AbilityChoicePool _abilityChoicePool;
    private UIEventManager _uIEventManager;
    private PlayerEventManager _playerEventManager;
    private EnemyEventManager _enemyEventManager;
    [Inject] private IGamePause _gamePause;
    private bool _isActive = true;

    private void Start()
    {
        _uIEventManager = UIEventManager.Instance;
        _playerEventManager = PlayerEventManager.Instance;
        _enemyEventManager = EnemyEventManager.Instance;
        _playerEventManager.PlayerDeath += HandlePlayerDeath;
        _enemyEventManager.BossDie += HandleBossDeath;
        _uIEventManager.AbilityChoice += HandleLevelIncreased;

        // Subscribe to the ButtonObtainedFromPool event
        _abilityChoicePool.ButtonObtainedFromPool += HandleButtonObtainedFromPool;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    protected override void Run()
    {

        if (_gamePause.IsPaused && _gamePause.IsActiveLines)
        {
            EnablingCanvases(false);
            _isActive = false;
            return;
        }
        else
        {
            if (!_isActive)
            {
                EnablingCanvases(true);
                _isActive = true;
            }
        }
    }

    private void EnablingCanvases(bool isActive)
    {
        _allUi.gameObject.SetActive(isActive);
    }

    private void UnsubscribeEvents()
    {
        _playerEventManager.PlayerDeath -= HandlePlayerDeath;
        _enemyEventManager.BossDie -= HandleBossDeath;
        _uIEventManager.AbilityChoice -= HandleLevelIncreased;
        _abilityChoicePool.ButtonObtainedFromPool -= HandleButtonObtainedFromPool;
    }

    private void HandlePlayerDeath()
    {
        // Activate the restart game panel
        Time.timeScale = 0f; // Pause the game
        _restartGamePanel.gameObject.GetComponent<GameOverPanel>().Victory = false;
        _restartGamePanel.SetActive(true);
    }

    private void HandleBossDeath()
    {
        // Activate the restart game panel
        Time.timeScale = 0f; // Pause the game
        _restartGamePanel.gameObject.GetComponent<GameOverPanel>().Victory = true;
        _restartGamePanel.SetActive(true);

        _gamePause.SetPause(true);
        _abilitySelectionPanel.SetActive(false);
        _talentTreePanel.SetActive(false);
        _joystickPanel.SetActive(false);
    }

    private void HandleLevelIncreased()
    {
        // Activate the ability selection panel
        _gamePause.SetPause(true);
        _abilitySelectionPanel.SetActive(true);
        _talentTreePanel.SetActive(true);
        _joystickPanel.SetActive(false);
    }

    private void HandleButtonObtainedFromPool(Button button)
    {
        // Add listener to the button
        button.onClick.AddListener(HandleAbilityButtonClicked);
    }

    private void HandleAbilityButtonClicked()
    {
        // Hide the ability selection panel
        _abilitySelectionPanel.SetActive(false);
        _talentTreePanel.SetActive(false);
        _joystickPanel.SetActive(true);
        // Remove the pause by resuming the game
        _gamePause.SetPause(false);
    }
}
