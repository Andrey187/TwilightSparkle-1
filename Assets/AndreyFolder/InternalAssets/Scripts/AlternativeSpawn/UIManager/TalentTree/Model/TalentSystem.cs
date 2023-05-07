using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class TalentSystem : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<TalentSO> _talents;
    private int _maxTalentPoints;
    private int _currentTalentPointsValue;
    public int TalentPoints { get => _playerStats.TalentPoints; set => _playerStats.TalentPoints = value; }
    public int MaxTalentPoints
    {
        get => _maxTalentPoints;
        set
        {
            _maxTalentPoints = value;
        }
    }

    public int CurrentTalentPointsValue
    {
        get => _currentTalentPointsValue;
        set
        {
            _currentTalentPointsValue = value;
        }
    }

    public void Upgrade(TalentStatType talentStatType,int buttonValue, int pointsInvested)
    {
        if (TalentPoints != 0 && CurrentTalentPointsValue != MaxTalentPoints)
        {
            var matchingTalents = _talents.Where(t => t.StatType == talentStatType);

            foreach (var talent in matchingTalents)
            {
                talent.UpdateTalent(_playerStats, buttonValue);
            }
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