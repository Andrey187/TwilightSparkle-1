using System.ComponentModel;
using UnityEngine;

public class EnemiesKillChallenge : BaseChallenge
{
    [SerializeField] private Counters _counters;
    [SerializeField] private LevelUpSystem _levelUpSystem;
    [SerializeField] private int _gainExpReward = 5000;

    protected internal override event PropertyChangedEventHandler PropertyChanged;
    protected override void Awake()
    {
        base.Awake();
        _levelUpSystem = FindObjectOfType<LevelUpSystem>();
        _counters.PropertyChanged += HandlePropertyChanged;
    }

    protected internal override int CurrentCountValue
    {
        get => _counters.KilledEnemy;
        set
        {
            _counters.KilledEnemy = value;
            OnPropertyChanged(nameof(CurrentCountValue));
        }
    }

    protected internal override int Reward { get => _gainExpReward; set => _gainExpReward = value; }
    protected internal override int MaxCountValue { get => _maxCountValue; set => _maxCountValue = value; }

    protected override void OnPropertyChanged(string propertyName) 
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_counters.KilledEnemy):
                OnPropertyChanged(nameof(CurrentCountValue));
                break;
        }
    }

    protected internal override void ChallengeReward()
    {
        CurrentCountValue = 0;
        _levelUpSystem.AddExperienceForChallenge(Reward);
    }
}

