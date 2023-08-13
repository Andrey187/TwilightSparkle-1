using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class TalentSystem : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<TalentSO> _talents;
    private Dictionary<TalentStatType, int> _maxTalentPoints = new Dictionary<TalentStatType, int>();
    private Dictionary<TalentStatType, int> _currentTalentPointsValue = new Dictionary<TalentStatType, int>();
    public int TalentPoints
    {
        get => _playerStats.TalentPoints;
        set
        {
            _playerStats.TalentPoints = value;
        }
    }
    public Dictionary<TalentStatType, int> MaxTalentPoints
    {
        get { return _maxTalentPoints; }
        set { _maxTalentPoints = value; }
    }

    public Dictionary<TalentStatType, int> CurrentTalentPointsValue
    {
        get { return _currentTalentPointsValue; }
        set { _currentTalentPointsValue = value;  }
    }

    private void Start()
    {
        foreach(var talent in _talents)
        {
            talent.ResetOnExitPlay();
            talent.PropertyChanged += HandlePropertyChanged;
            _maxTalentPoints.Add(talent.StatType, talent.MaxTalentPoints);
            _currentTalentPointsValue.Add(talent.StatType, talent.CurrentTalentPoint);
        }
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        foreach (var talents in _talents)
        {
            talents.PropertyChanged -= HandlePropertyChanged;
        }
        _maxTalentPoints.Clear();
        _currentTalentPointsValue.Clear();
    }

    public void Upgrade(TalentStatType talentStatType, int buttonValue, int pointsInvested)
    {
        if (TalentPoints != 0 && _currentTalentPointsValue[talentStatType] != _maxTalentPoints[talentStatType])
        {
            var matchingTalents = _talents.Where(t => t.StatType == talentStatType);

            foreach (var talent in matchingTalents)
            {
                talent.UpdateTalent(_playerStats, buttonValue);
                TalentPoints -= pointsInvested;
                talent.CurrentTalentPoint++;
            }
            OnPropertyChanged(nameof(TalentPoints));
        }
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is TalentSO talentSO)
        {
            // Check if the changed property is MaxCountValue or CurrentCountValue
            if (e.PropertyName == nameof(TalentSO.MaxTalentPoints) || e.PropertyName == nameof(TalentSO.CurrentTalentPoint))
            {
                _maxTalentPoints[talentSO.StatType] = talentSO.MaxTalentPoints;
                _currentTalentPointsValue[talentSO.StatType] = talentSO.CurrentTalentPoint;
                OnPropertyChanged(nameof(MaxTalentPoints));
                OnPropertyChanged(nameof(CurrentTalentPointsValue));
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}