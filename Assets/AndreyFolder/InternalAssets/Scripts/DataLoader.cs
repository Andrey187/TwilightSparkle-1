using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoCache
{
    [SerializeField] public AbilityDatabase _abilityDataBase;
    [SerializeField] public DoTDataBase _dotDataBase;

    private Dictionary<string, AbilityData> abilityDataDictionary = new Dictionary<string, AbilityData>();
    private Dictionary<string, DoTData> _dotDataBaseDictionary = new Dictionary<string, DoTData>();

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
    }

    public void InitializeAbilities()
    {
        abilityDataDictionary.Clear();

        foreach (AbilityData abilityData in _abilityDataBase.AbilityDataList)
        {
            abilityDataDictionary.Add(abilityData.AbilityName, abilityData);
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

    public AbilityData GetAbilityData(string abilityName)
    {
        if (abilityDataDictionary.TryGetValue(abilityName, out AbilityData abilityData))
        {
            Debug.Log(abilityData);
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
            Debug.Log(dotData);
            return dotData;
        }
        else
        {
            Debug.LogError($"AbilityData not found for ability name '{dotName}'");
            return null;
        }
    }
}
