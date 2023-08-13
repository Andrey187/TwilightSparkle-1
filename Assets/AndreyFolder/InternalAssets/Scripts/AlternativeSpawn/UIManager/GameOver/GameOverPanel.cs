using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textKillEnemy;
    private Counters _counters;

    private void Start()
    {
        _counters = FindObjectOfType<Counters>();
        _textKillEnemy.SetText(_counters._totalKilledEnemy.ToString());
    }
}
