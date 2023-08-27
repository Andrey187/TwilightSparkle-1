using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    private Button _resumeButton;
    private SceneLoadManager _sceneLoadManager;

    private void Awake()
    {
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        Debug.Log(_sceneLoadManager + " _loadMenuSettings");
        _resumeButton = gameObject.GetComponent<Button>();
        _resumeButton.onClick.AddListener(SettingsButton);
    }

    private void SettingsButton()
    {
        _sceneLoadManager.LoadMenuScene("MenuScene");
    }
}
