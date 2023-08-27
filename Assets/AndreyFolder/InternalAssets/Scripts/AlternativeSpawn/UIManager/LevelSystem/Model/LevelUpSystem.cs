using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class LevelUpSystem : MonoBehaviour, INotifyPropertyChanged, ILevelUpSystem
{
    public event PropertyChangedEventHandler PropertyChanged;
    [Inject] private IPlayerStats _playerStats;
    [SerializeField] private int _gainExpMyltiply = 1;
    [SerializeField] private float expFactor = 1.3f;
    [SerializeField] private float levelFactor = 0.3f;
    private Dictionary<EnemyData.ObjectType, EnemyData> _expTable = new Dictionary<EnemyData.ObjectType, EnemyData>();
    private int _talentPoints;

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

    public int GainExpMyltiply { get => _gainExpMyltiply; set => _gainExpMyltiply = value; }

    public int NextLevelExp { get; set; } = 1000;

    public void AddExperience(EnemyData.ObjectType type, EnemyData enemyType)
    {
        if (!_expTable.ContainsKey(type))
        {
            _expTable.Add(type, enemyType);
        }

        if (_expTable.TryGetValue(type, out EnemyData enemy))
        {
            int expMyltiply = Mathf.RoundToInt(enemy.GainExp * GainExpMyltiply / 100f);
            int gainExp = enemy.GainExp + expMyltiply;
            _playerStats.CurrentExp += gainExp;
            OnPropertyChanged(nameof(CurrentExp));
            CheckLevelUp();
        }
    }

    public void AddExperienceForChallenge(int gainExp)
    {
        _playerStats.CurrentExp += gainExp;
        OnPropertyChanged(nameof(CurrentExp));
        CheckLevelUp();
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
        const float reductionFactor = 0.9f;  // Gradual reduction factor for growth rate

        float exp = baseExp * Mathf.Pow(expFactor, level - 1);
        float levelBonus = (level - 1) * levelFactor * exp;

        int result = Mathf.FloorToInt(exp + levelBonus);

        if (level % 10 == 0)
        {
            int reductionLevel = level / 10;  // Calculate reduction level
            float growthReduction = Mathf.Pow(reductionFactor, reductionLevel);
            result = Mathf.FloorToInt(result * growthReduction);
        }

        return result;
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
