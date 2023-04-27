using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour, INotifyPropertyChanged
{
    private static LevelUpSystem _instance;
    private Dictionary<EnemyType.ObjectType, EnemyType> _expTable = new Dictionary<EnemyType.ObjectType, EnemyType>();
    public event Action<int> OnCurrentLevel;
    public event Action<int> OnCurrentExp;
    public event Action<int> OnNextLevelExp;
    public event Action<int> OnTalantPoint;
    public event PropertyChangedEventHandler PropertyChanged;

    public int CurrentLevel { get; set; } = 1;

    public int CurrentExp { get; set; } = 0;

    public int NextLevelExp { get; set; } = 1000;

    public int TalantPoint { get; set; } = 0;

    public static LevelUpSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelUpSystem>();
            return _instance;
        }
    }

    public void AddExperience(EnemyType.ObjectType type, EnemyType enemyType)
    {
        if (!_expTable.ContainsKey(type))
        {
            _expTable.Add(type, enemyType);
        }

        if (_expTable.TryGetValue(type, out EnemyType enemy))
        {
            int gainExp = enemy.GainExp;
            CurrentExp += gainExp;
            OnCurrentExp?.Invoke(CurrentExp);
            OnPropertyChanged(nameof(CurrentExp));
            CheckLevelUp();
        }
    }

    private void CheckLevelUp()
    {
        if (CurrentExp >= NextLevelExp)
        {
            CurrentLevel++;
            TalantPoint++;
            CurrentExp -= NextLevelExp;
            
            NextLevelExp = GetNextLevelExp(CurrentLevel);

            OnCurrentExp?.Invoke(CurrentExp);
            OnPropertyChanged(nameof(CurrentExp));

            OnCurrentLevel?.Invoke(CurrentLevel);
            OnPropertyChanged(nameof(CurrentLevel));

            OnTalantPoint?.Invoke(TalantPoint);
            OnPropertyChanged(nameof(TalantPoint));

            OnNextLevelExp?.Invoke(NextLevelExp);
            OnPropertyChanged(nameof(NextLevelExp));

        }
    }

    private int GetNextLevelExp(int level)
    {
        const float baseExp = 1000f;
        const float expFactor = 1.5f;
        const float levelFactor = 0.2f;

        float exp = baseExp * Mathf.Pow(expFactor, level - 1);
        float levelBonus = level * levelFactor * exp;

        return Mathf.FloorToInt(exp + levelBonus);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
