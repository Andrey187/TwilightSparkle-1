using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private float expFactor = 1.3f;
    [SerializeField] private float levelFactor = 0.3f;
    private static LevelUpSystem _instance;
    private Dictionary<EnemyData.ObjectType, EnemyData> _expTable = new Dictionary<EnemyData.ObjectType, EnemyData>();
    private int _talentPoints;

    public static LevelUpSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelUpSystem>();
            return _instance;
        }
    }

    public int TalentPoint
    {
        get => _playerStats.TalentPoints;
        set
        {
            if (_talentPoints != value)
            {
                _talentPoints = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TalentPoint)));
            }
        }
    }

    public int CurrentLevel { get => _playerStats.CurrentLevel; set =>_playerStats.CurrentLevel = value; }

    public int CurrentExp { get => _playerStats.CurrentExp; set => _playerStats.CurrentExp = value; }

    public int NextLevelExp { get; set; } = 1000;

    public void AddExperience(EnemyData.ObjectType type, EnemyData enemyType)
    {
        if (!_expTable.ContainsKey(type))
        {
            _expTable.Add(type, enemyType);
        }

        if (_expTable.TryGetValue(type, out EnemyData enemy))
        {
            int gainExp = enemy.GainExp;
            _playerStats.CurrentExp += gainExp;
            OnPropertyChanged(nameof(CurrentExp));
            CheckLevelUp();
        }
    }

    private void CheckLevelUp()
    {
        while (_playerStats.CurrentExp >= NextLevelExp)
        {
            _playerStats.CurrentLevel++;
            _playerStats.TalentPoints++;
            _playerStats.CurrentExp -= NextLevelExp;
            
            NextLevelExp = GetNextLevelExp(CurrentLevel);

            OnPropertyChanged(nameof(CurrentExp));
            OnPropertyChanged(nameof(CurrentLevel));
            OnPropertyChanged(nameof(NextLevelExp));
            OnPropertyChanged(nameof(TalentPoint));
        }
    }

    private int GetNextLevelExp(int level)
    {
        const float baseExp = 1000f;
        //const float expFactor = 1.3f;
        //const float levelFactor = 0.3f;

        float exp = baseExp * Mathf.Pow(expFactor, level - 1);
        float levelBonus = (level - 1) * levelFactor * exp;

        return Mathf.FloorToInt(exp + levelBonus);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
