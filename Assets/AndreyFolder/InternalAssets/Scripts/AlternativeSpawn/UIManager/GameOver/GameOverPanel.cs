using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _textKillEnemy;
    private Counters _counters;
    private SceneLoadManager _sceneLoadManager;

    private void Start()
    {
        _counters = FindObjectOfType<Counters>();
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
        _textKillEnemy.SetText(_counters._killedEnemy.ToString());
        _restartButton.onClick.AddListener(RestartScene);
    }

    private void RestartScene()
    {
        _sceneLoadManager.RestartGame();
    }
}
