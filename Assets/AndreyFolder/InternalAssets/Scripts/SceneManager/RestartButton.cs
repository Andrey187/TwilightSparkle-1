using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button _resumeButton;
    private SceneLoadManager _sceneLoadManager;

    private void Awake()
    {
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        _resumeButton = gameObject.GetComponent<Button>();
        _resumeButton.onClick.AddListener(Restart);
    }

    private async void Restart()
    {
        await _sceneLoadManager.RestartGameAddressablesMethod();
    }
}
