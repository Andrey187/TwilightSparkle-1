using AbilitySystem;
using System.ComponentModel;
using UnityEngine;

public class FireBallCollectChallenge : BaseChallenge
{
    protected internal override event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private AttackSystem _attackSystem;
    [SerializeField] private BaseAbilities _fireBall;
    [SerializeField] private BaseAbilities _multipleFireBall;
    [SerializeField] private int _gainFireBall;
    private int _currentValue = 0;
    protected override void Awake()
    {
        base.Awake();
        _attackSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackSystem>();
        _fireBall.SetCreate += IncrementCurrentValue;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        _fireBall.SetCreate -= IncrementCurrentValue;
    }

    protected internal override int Reward { get => _gainFireBall; set => _gainFireBall = value; }
    protected internal override int MaxCountValue { get => _maxCountValue; set => _maxCountValue = value; }
    protected internal override int CurrentCountValue { get => _currentValue; set => _currentValue = value; }

    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void IncrementCurrentValue()
    {
        _currentValue += 1;
        OnPropertyChanged(nameof(CurrentCountValue));
    }

    protected internal override void ChallengeReward()
    {
        CurrentCountValue = 0;
        _attackSystem.AddAlternativeAbility(_multipleFireBall);
        OnPropertyChanged(nameof(CurrentCountValue));
    }
}
