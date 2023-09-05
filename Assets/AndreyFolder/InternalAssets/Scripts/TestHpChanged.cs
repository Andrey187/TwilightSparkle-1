using UnityEngine;
using UnityEngine.UI;

public class TestHpChanged : MonoBehaviour
{
    [SerializeField] private PlayerStats PlayerStats;
    [SerializeField] private int _countHp;
    [SerializeField] private Button button;


    private void Start()
    {
        button.onClick.AddListener(() => ChangedHP(_countHp));
    }

    private void ChangedHP(int value)
    {
        PlayerStats.CurrentHealth += _countHp;
    }
}
