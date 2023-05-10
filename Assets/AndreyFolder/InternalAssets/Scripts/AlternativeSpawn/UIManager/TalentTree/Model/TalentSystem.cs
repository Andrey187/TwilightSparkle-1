using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class TalentSystem : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<TalentStatType, int> OnCurrentTalentPoint;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<TalentSO> _talents;
    private Dictionary<TalentStatType, int> _maxTalentPoints = new Dictionary<TalentStatType, int>();
    private Dictionary<TalentStatType, int> _currentTalentPointsValue = new Dictionary<TalentStatType, int>();
    public int TalentPoints { get => _playerStats.TalentPoints; set => _playerStats.TalentPoints = value; }
    public Dictionary<TalentStatType, int> MaxTalentPoints
    {
        get { return _maxTalentPoints; }
        set { _maxTalentPoints = value; }
    }

    public Dictionary<TalentStatType, int> CurrentTalentPointsValue
    {
        get { return _currentTalentPointsValue; }
        set { _currentTalentPointsValue = value; }
    }

    public void Upgrade(TalentStatType talentStatType,int buttonValue, int pointsInvested)
    {
        if (TalentPoints != 0 && _currentTalentPointsValue[talentStatType] != _maxTalentPoints[talentStatType])
        {
            var matchingTalents = _talents.Where(t => t.StatType == talentStatType);

            foreach (var talent in matchingTalents)
            {
                talent.UpdateTalent(_playerStats, buttonValue);
                TalentPoints -= pointsInvested;
                talent.CurrentTalentPoint++;
                OnCurrentTalentPoint?.Invoke(talentStatType,talent.CurrentTalentPoint);
                _currentTalentPointsValue[talentStatType]++;
            }
            OnPropertyChanged(nameof(TalentPoints));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}