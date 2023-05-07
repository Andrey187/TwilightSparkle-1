using System;
using System.ComponentModel;
using UnityEngine;

public class TalentSystem : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField] private PlayerStats _playerStats;
    public event PropertyChangedEventHandler PropertyChanged;
    public int MaxHealth  { get { return _playerStats.MaxHealth; } set { _playerStats.MaxHealth = value; } }
    public int TalentPoints { get { return _playerStats.TalentPoints; } set { _playerStats.TalentPoints = value; } }

    private int _maxTalentPoints;
    public int MaxTalentPoints
    {
        get => _maxTalentPoints;
        set
        {
            _maxTalentPoints = value;
        }
    }

    private int _currentTalentPointsValue;
    public int CurrentTalentPointsValue
    {
        get => _currentTalentPointsValue;
        set
        {
            _currentTalentPointsValue = value;
        }
    }

    public void Upgrade(int buttonValue, int pointsInvested)
    {
        if (TalentPoints != 0 && CurrentTalentPointsValue != MaxTalentPoints)
        {
            MaxHealth += buttonValue;
            TalentPoints -= pointsInvested;
            _currentTalentPointsValue++;
            OnPropertyChanged(nameof(TalentPoints));
            OnPropertyChanged(nameof(CurrentTalentPointsValue));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}