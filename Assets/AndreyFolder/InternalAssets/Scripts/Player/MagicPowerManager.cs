using UnityEngine;

public class MagicPowerManager : MonoBehaviour
{
    [SerializeField] private DataLoader _dataLodader;
    [SerializeField] private PlayerStats _playerStats;

    private void Start()
    {
        _dataLodader = FindObjectOfType<DataLoader>();
        _playerStats = FindObjectOfType<PlayerStats>();
        _playerStats.MagicPowerChanged += DamageUp;
    }

    public void DamageUp(int damage)
    {
        foreach (var a in _dataLodader._abilityDataBase.AbilityDataList)
        {
            a.OnMagicPowerChanged(damage);
        }
    }
}
