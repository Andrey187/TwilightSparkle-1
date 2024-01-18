using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _textKillEnemy;
    [SerializeField] private GameObject _exitButton;
    private Counters _counters;

    public bool Victory { get; set; } = false;

    private void Start()
    {
        _counters = FindObjectOfType<Counters>();
        _textKillEnemy.SetText(_counters._totalKilledEnemy.ToString());

        if (Victory)
        {
            VictoryGame();
        }
        else
        {
            GameOver();
        }
    }

    private void VictoryGame()
    {
        _headerText.SetText("Victory");
        _exitButton.SetActive(true);
        _headerText.color = Color.green;
    }

    private void GameOver()
    {
        _headerText.SetText("GameOver");
        _headerText.color = Color.red;
    }
}
