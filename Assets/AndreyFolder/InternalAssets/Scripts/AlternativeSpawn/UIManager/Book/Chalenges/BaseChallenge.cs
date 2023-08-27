using System.ComponentModel;
using UnityEngine;

public abstract class BaseChallenge : MonoBehaviour, IResetOnExitPlay
{
    [SerializeField] protected internal ChallengeType ChallengeType;
    [SerializeField] protected int _maxCountValue;
    protected internal string Description;
    protected internal abstract int Reward { get; set; }
    protected internal abstract int CurrentCountValue { get; set; }
    protected internal abstract int MaxCountValue { get; set; }
    protected internal abstract event PropertyChangedEventHandler PropertyChanged;
    protected int StartingCurrentCountValue = 0; // Add a new property to store the initial value

    protected virtual void Awake()
    {
        ResetOnExitPlay();
    }

    public void ResetOnExitPlay()
    {
        CurrentCountValue = StartingCurrentCountValue;
    }

    protected internal abstract void ChallengeReward();

    protected virtual void OnPropertyChanged(string propertyName) { }
    
}
