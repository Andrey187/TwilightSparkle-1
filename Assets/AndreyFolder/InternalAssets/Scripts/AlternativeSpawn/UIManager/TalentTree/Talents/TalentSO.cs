using System.ComponentModel;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Talents/New Talent")]
public class TalentSO : ScriptableObject, IResetOnExitPlay
{
    public TalentStatType StatType;
    public string Name;
    public string Description;
    [SerializeField] private int _currentTalentPoints;
    [SerializeField] private int _maxTalentPoints;
    [SerializeField] private int _defaultCurrentTalentPoint = 0;
    [SerializeField] private int _defaultMaxTalentPoint = 0;
    public event PropertyChangedEventHandler PropertyChanged;


    public int CurrentTalentPoint
    {
        get => _currentTalentPoints;
        set
        {
            _currentTalentPoints = value;
            OnPropertyChanged(nameof(CurrentTalentPoint));
        }
    }

    public int MaxTalentPoints { get => _maxTalentPoints;
        set
        {
            _maxTalentPoints = value;
        }
    }

    public void ChangeMaxTalentPoints(int value)
    {
        _maxTalentPoints += value;
        OnPropertyChanged(nameof(MaxTalentPoints));
    }

    public void UpdateTalent(IPlayerStats playerStats, int buttonValue)
    {
        PropertyInfo prop = typeof(IPlayerStats).GetProperty(StatType.ToString());
        int currentValue = (int)prop.GetValue(playerStats);
        prop.SetValue(playerStats, currentValue + buttonValue);
    }

    public void ResetOnExitPlay()
    {
        CurrentTalentPoint = _defaultCurrentTalentPoint;
        MaxTalentPoints = _defaultMaxTalentPoint;
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
