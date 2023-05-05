using System;
using System.ComponentModel;
using UnityEngine;

public class TalentSystem : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField] private PlayerStats _playerStats;
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<int> TalentPointsChanged;

    public int MaxHealth  { get { return _playerStats.MaxHealth; } set { _playerStats.MaxHealth = value; } }
    public int TalentPoints { get { return _playerStats.TalentPoints; } set { _playerStats.TalentPoints = value; } }

    public void Upgrade(int buttonValue, int pointsInvested)
    {
        if (TalentPoints != 0)
        {
            MaxHealth += buttonValue;
            TalentPoints -= pointsInvested;
            TalentPointsChanged?.Invoke(TalentPoints);
            OnPropertyChanged(nameof(TalentPoints));
            Debug.Log("TalentPointsChanged event raised. New TalentPoints value: " + TalentPoints);
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}