using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoCache
{
    [SerializeField] public AbilityDatabase _abilityDataBase;
    [SerializeField] public DoTDataBase _dotDataBase;
    [SerializeField] public EnemyDataBase _enemyDataBase;

    private Dictionary<string, AbilityData> _abilityDataDictionary = new Dictionary<string, AbilityData>();
    private Dictionary<string, DoTData> _dotDataBaseDictionary = new Dictionary<string, DoTData>();
    private Dictionary<string, EnemyData> _enemyDataBaseDictionary = new Dictionary<string, EnemyData>();

    private static DataLoader instance;
    public static DataLoader Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DataLoader>();
            return instance;
        }
    }

    private void Awake()
    {
        InitializeAbilities();
        InitializeDoTs();
        InitializeEnemies();
    }

    public void InitializeAbilities()
    {
        _abilityDataDictionary.Clear();

        foreach (AbilityData abilityData in _abilityDataBase.AbilityDataList)
        {
            _abilityDataDictionary.Add(abilityData.AbilityName, abilityData);
            abilityData.ResetOnExitPlay();
        }
    }

    public void InitializeDoTs()
    {
        _dotDataBaseDictionary.Clear();

        foreach (DoTData dotData in _dotDataBase.DoTDataList)
        {
            _dotDataBaseDictionary.Add(dotData.DoTName, dotData);
        }
    }

    public void InitializeEnemies()
    {
        _enemyDataBaseDictionary.Clear();
        foreach (EnemyData enemyData in _enemyDataBase.EnemyDataList)
        {
            _enemyDataBaseDictionary.Add(enemyData.EnemyName, enemyData);
            enemyData.ResetOnExitPlay();
        }
    }

    public AbilityData GetAbilityData(string abilityName)
    {
        if (_abilityDataDictionary.TryGetValue(abilityName, out AbilityData abilityData))
        {
            return abilityData;
        }
        else
        {
            Debug.LogError($"AbilityData not found for ability name '{abilityName}'");
            return null;
        }
    }

    public DoTData GetDoTData(string dotName)
    {
        if (_dotDataBaseDictionary.TryGetValue(dotName, out DoTData dotData))
        {
            return dotData;
        }
        else
        {
            Debug.LogError($"AbilityData not found for ability name '{dotName}'");
            return null;
        }
    }
}
