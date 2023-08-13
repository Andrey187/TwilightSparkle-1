using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseChallengeView : MonoBehaviour
{
    [SerializeField] protected ChallengeType _statType;
    [SerializeField] protected BookStatsModel _bookStatsModel;
    [SerializeField] protected TextMeshProUGUI _nameText;
    [SerializeField] protected TextMeshProUGUI _currentCountValueText;
    [SerializeField] protected TextMeshProUGUI _maxCountValueText;
    [SerializeField] protected TextMeshProUGUI _rewardDescription;
    [SerializeField] protected Button _button;
    protected BookStatsViewModel _bookStatsViewModel;
    protected AttentionController _attentionController;
    protected AutoClickButton _autoClickButton;

    protected Dictionary<ChallengeType, Button> _buttons = new Dictionary<ChallengeType, Button>();
    protected Dictionary<ChallengeType, int> _reward = new Dictionary<ChallengeType, int>();
    protected Dictionary<ChallengeType, int> _currentCount = new Dictionary<ChallengeType, int>();
    protected Dictionary<ChallengeType, int> _maxCount = new Dictionary<ChallengeType, int>();

    protected virtual void Awake()
    {
        _bookStatsViewModel = new BookStatsViewModel(_bookStatsModel);

        _attentionController = _bookStatsViewModel.AttentionController;
        _autoClickButton = _bookStatsViewModel.AutoClickButton;

        foreach (var value in _bookStatsViewModel.CurrentCountValue)
        {
            _currentCount.Add(value.Key, value.Value);
        }

        foreach (var value in _bookStatsViewModel.MaxCountValue)
        {
            _maxCount.Add(value.Key, value.Value);
        }

        foreach (var value in _bookStatsViewModel.RewardDescription)
        {
            _reward.Add(value.Key, value.Value);
        }

        _buttons.Add(_statType, _button);

        _buttons[_statType].onClick.AddListener(() => _bookStatsViewModel.OnButtonClick(_statType));

        _bookStatsViewModel.PropertyChanged += HandlePropertyChanged;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    protected void UnsubscribeEvents()
    {
        _bookStatsViewModel.PropertyChanged -= HandlePropertyChanged;
        _currentCount.Clear();
        _reward.Clear();
        _maxCount.Clear();
        _buttons.Clear();
    }

    protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is BookStatsViewModel bookStatsViewModel)
        {
            switch (e.PropertyName)
            {
                case nameof(bookStatsViewModel.CurrentCountValue):
                    if (bookStatsViewModel.CurrentCountValue.TryGetValue(_statType, out int currentCountValue))
                    {
                        _currentCount[_statType] = currentCountValue;
                        _currentCountValueText.SetText(_currentCount[_statType].ToString());
                        SetCurrentTalentPoint(_statType);
                    }
                    break;
                case nameof(bookStatsViewModel.MaxCountValue):
                    if (bookStatsViewModel.MaxCountValue.TryGetValue(_statType, out int maxCountValue))
                    {
                        _maxCount[_statType] = maxCountValue;
                        _maxCountValueText.SetText(_maxCount[_statType].ToString());
                    }
                    break;
            }
        }
    }

    protected void SetCurrentTalentPoint(ChallengeType statType)
    {
        if (_statType == statType)
        {
            if (_currentCount[_statType] >= _maxCount[_statType])
            {
                _buttons[_statType].interactable = true;
                _attentionController.ObjectActivate();

                if (_autoClickButton.AutoEnable == true)
                {
                    _bookStatsViewModel.OnButtonClick(_statType);
                }
            }
            else { _buttons[_statType].interactable = false; }
        }
    }
}
