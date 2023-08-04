using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Button _resumeButton;
    private SceneLoadManager _sceneLoadManager;

    private void Awake()
    {
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        Debug.Log(_sceneLoadManager + " _sceneLoadManager");
        _resumeButton = gameObject.GetComponent<Button>();
        _resumeButton.onClick.AddListener(Resume);
    }

    private void Resume()
    {
        _sceneLoadManager.ResumeInMainScene();
    }
}
