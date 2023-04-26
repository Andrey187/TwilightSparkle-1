using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private static LevelUpSystem _instance;
    public event Action<int> OnLevelUp;

    private int _currentLevel = 1;
    private int _currentExp = 0;
    private int _nextLevelExp = 1000;
    private int _talentPoints = 0;

    public static LevelUpSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelUpSystem>();
            return _instance;
        }
    }

    private Dictionary<EnemyType.ObjectType, EnemyType> _expTable = new Dictionary<EnemyType.ObjectType, EnemyType>();


    private void Start()
    {
        Debug.Log(_expTable.Keys.Count);
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
            _currentExp += gainExp;
            Debug.Log($"Gained {gainExp} experience. Total experience: {_currentExp}");

            CheckLevelUp();
        }
    }

    private void CheckLevelUp()
    {
        if (_currentExp >= _nextLevelExp)
        {
            _currentLevel++;
            _talentPoints++;
            _currentExp = 0;
            _nextLevelExp = GetNextLevelExp(_currentLevel);
            OnLevelUp?.Invoke(_currentLevel);
            Debug.Log($"Leveled up to level {_currentLevel}. Talent points: {_talentPoints}");

            if (_currentLevel <= _nextLevelExp)
            {
                Debug.Log($"Next level requires {_nextLevelExp} exp.");
            }
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

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetCurrentExp()
    {
        return _currentExp;
    }

    public int GetNextLevelExp()
    {
        return _nextLevelExp;
    }

    public int GetTalentPoints()
    {
        return _talentPoints;
    }
}
