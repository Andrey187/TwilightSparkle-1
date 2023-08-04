using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button _resumeButton;
    private SceneLoadManager _sceneLoadManager;

    private void Awake()
    {
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        _resumeButton = gameObject.GetComponent<Button>();
        _resumeButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        _sceneLoadManager.ExitGame();
    }
}
