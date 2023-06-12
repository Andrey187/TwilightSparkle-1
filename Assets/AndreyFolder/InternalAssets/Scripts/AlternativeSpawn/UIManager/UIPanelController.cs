using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private GameObject restartGamePanel;
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private AbilityChoicePool abilityChoicePool;
    private UIEventManager _uIEventManager;
    private PlayerEventManager _playerEventManager;
    private void Start()
    {
        _uIEventManager = UIEventManager.Instance;
        _playerEventManager = PlayerEventManager.Instance;
        _playerEventManager.PlayerDeath += HandlePlayerDeath;
        _uIEventManager.AbilityChoice += HandleLevelIncreased;

        // Subscribe to the ButtonObtainedFromPool event
        abilityChoicePool.ButtonObtainedFromPool += HandleButtonObtainedFromPool;
    }

    private void OnDisable()
    {
        _playerEventManager.PlayerDeath -= HandlePlayerDeath;
        _uIEventManager.AbilityChoice -= HandleLevelIncreased;
        abilityChoicePool.ButtonObtainedFromPool -= HandleButtonObtainedFromPool;
    }

    private void HandlePlayerDeath()
    {
        // Activate the restart game panel
        Time.timeScale = 0f; // Pause the game
        restartGamePanel.SetActive(true);
    }

    private void HandleLevelIncreased()
    {
        // Activate the ability selection panel
        Time.timeScale = 0f; // Pause the game
        abilitySelectionPanel.SetActive(true);
    }

    private void HandleButtonObtainedFromPool(Button button)
    {
        // Add listener to the button
        button.onClick.AddListener(HandleAbilityButtonClicked);
    }

    private void HandleAbilityButtonClicked()
    {
        // Hide the ability selection panel
        abilitySelectionPanel.SetActive(false);

        // Remove the pause by resuming the game
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Reset any necessary game state, variables, or managers
        // Load the initial scene or restart the current scene
        Time.timeScale = 1f;
        ObjectPoolManager.Instance.ClearAllPools();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
