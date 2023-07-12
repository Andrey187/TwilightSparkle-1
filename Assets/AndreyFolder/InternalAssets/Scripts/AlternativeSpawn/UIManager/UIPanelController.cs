using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _restartGamePanel;
    [SerializeField] private GameObject _abilitySelectionPanel;
    [SerializeField] private GameObject _talentTreePanel;
    [SerializeField] private GameObject _joystickPanel;
    [SerializeField] private AbilityChoicePool _abilityChoicePool;
    private UIEventManager _uIEventManager;
    private PlayerEventManager _playerEventManager;
    private void Start()
    {
        _uIEventManager = UIEventManager.Instance;
        _playerEventManager = PlayerEventManager.Instance;
        _playerEventManager.PlayerDeath += HandlePlayerDeath;
        _uIEventManager.AbilityChoice += HandleLevelIncreased;

        // Subscribe to the ButtonObtainedFromPool event
        _abilityChoicePool.ButtonObtainedFromPool += HandleButtonObtainedFromPool;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        _playerEventManager.PlayerDeath -= HandlePlayerDeath;
        _uIEventManager.AbilityChoice -= HandleLevelIncreased;
        _abilityChoicePool.ButtonObtainedFromPool -= HandleButtonObtainedFromPool;
    }

    private void HandlePlayerDeath()
    {
        // Activate the restart game panel
        Time.timeScale = 0f; // Pause the game
        _restartGamePanel.SetActive(true);
    }

    private void HandleLevelIncreased()
    {
        // Activate the ability selection panel
        Time.timeScale = 0f; // Pause the game
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
        Time.timeScale = 1f;
    }
}
