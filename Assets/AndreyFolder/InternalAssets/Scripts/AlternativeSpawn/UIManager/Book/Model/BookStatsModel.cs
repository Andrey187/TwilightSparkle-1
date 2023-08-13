using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class BookStatsModel : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [SerializeField] private List<BaseChallenge> _challenges;
    [SerializeField] private AttentionController _attentionController;
    [SerializeField] private AutoClickButton _autoClickButton;

    private Dictionary<ChallengeType, int> _rewardDescription = new Dictionary<ChallengeType, int>();
    private Dictionary<ChallengeType, int> _currentCountValue = new Dictionary<ChallengeType, int>();
    private Dictionary<ChallengeType, int> _maxCountValue = new Dictionary<ChallengeType, int>();

    public AutoClickButton AutoClickButton
    {
        get { return _autoClickButton; }
        set { _autoClickButton = value; }
    }

    public AttentionController AttentionController
    {
        get { return _attentionController; }
        set { _attentionController = value; }
    }

    public Dictionary<ChallengeType, int> RewardDescription
    {
        get { return _rewardDescription; }
        set { _rewardDescription = value; }
    }

    public Dictionary<ChallengeType, int> MaxCountValue
    {
        get { return _maxCountValue; }
        set { _maxCountValue = value; }
    }

    public Dictionary<ChallengeType, int> CurrentCountValue
    {
        get { return _currentCountValue; }
        set { _currentCountValue = value; }
    }

    private void Awake()
    {
       
        foreach (var challenge in _challenges)
        {
            challenge.PropertyChanged += HandlePropertyChanged;
            _rewardDescription.Add(challenge.ChallengeType, challenge.Reward);
            _currentCountValue.Add(challenge.ChallengeType, challenge.CurrentCountValue);
            _maxCountValue.Add(challenge.ChallengeType, challenge.MaxCountValue);
        }
        _attentionController = FindObjectOfType<AttentionController>();
        _autoClickButton = FindObjectOfType<AutoClickButton>();
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        foreach (var challenge in _challenges)
        {
            challenge.PropertyChanged -= HandlePropertyChanged;
        }
        _currentCountValue.Clear();
        _maxCountValue.Clear();
        _rewardDescription.Clear();
    }

    public void Reward(ChallengeType challengeType)
    {
        var matchingChallenge = _challenges.Where(t => t.ChallengeType == challengeType);
        foreach (var challenge in matchingChallenge)
        {
            challenge.ChallengeReward();
            _attentionController.ObjectDeactivate();
            OnPropertyChanged(nameof(CurrentCountValue));
        }
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is BaseChallenge challenge)
        {
            // Check if the changed property is MaxCountValue or CurrentCountValue
            if (e.PropertyName == nameof(BaseChallenge.MaxCountValue) || e.PropertyName == nameof(BaseChallenge.CurrentCountValue))
            {
                _currentCountValue[challenge.ChallengeType] = challenge.CurrentCountValue;
                _maxCountValue[challenge.ChallengeType] = challenge.MaxCountValue;

                OnPropertyChanged(nameof(CurrentCountValue));
                OnPropertyChanged(nameof(MaxCountValue));
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
