using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private GameObject restartGamePanel;
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private AbilityChoicePool abilityChoicePool;
    private EventManager eventManager;

    private void Start()
    {
        eventManager = EventManager.Instance;
        eventManager.PlayerDeath += HandlePlayerDeath;
        eventManager.AbilityChoice += HandleLevelIncreased;

        // Subscribe to the ButtonObtainedFromPool event
        abilityChoicePool.ButtonObtainedFromPool += HandleButtonObtainedFromPool;
    }

    private void OnDisable()
    {
        eventManager.PlayerDeath -= HandlePlayerDeath;
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
