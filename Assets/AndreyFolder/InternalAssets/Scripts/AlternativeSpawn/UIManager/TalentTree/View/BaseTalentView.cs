using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTalentView: MonoBehaviour 
{
    [SerializeField] protected TalentStatType _statType;
    [SerializeField] protected TalentSystem _talentSystem;
    [SerializeField] protected TextMeshProUGUI _nameText;
    [SerializeField] protected TextMeshProUGUI _buttonText;
    [SerializeField] protected TextMeshProUGUI _currentTalentPointText;
    [SerializeField] protected TextMeshProUGUI _maxTalentPointsText;
    [SerializeField] protected Button _button;
    [SerializeField] protected int _buttonTextValue;
    protected TalentViewModel _talentViewModel;
    protected Dictionary<TalentStatType, Button> _buttons = new Dictionary<TalentStatType, Button>();
    protected Dictionary<TalentStatType, int> _currentTalent = new Dictionary<TalentStatType, int>();
    protected Dictionary<TalentStatType, int> _maxTalent = new Dictionary<TalentStatType, int>();

    protected virtual void Start()
    {
        _talentViewModel = new TalentViewModel(_talentSystem);

        foreach (var value in _talentViewModel.MaxTalentPoints)
        {
            _maxTalent.Add(value.Key, value.Value);
        }

        foreach (var value in _talentViewModel.CurrentTalentPointsValue)
        {
            _currentTalent.Add(value.Key, value.Value);
        }

        _talentViewModel.PropertyChanged += HandlePropertyChanged;

        _buttons.Add(_statType, _button);
        _buttons[_statType].onClick.AddListener(() => _talentViewModel.OnButtonClick(_statType, _buttonTextValue));
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }
    protected void UnsubscribeEvents()
    {
        _talentSystem.PropertyChanged -= HandlePropertyChanged;
        _currentTalent.Clear();
        _maxTalent.Clear();
        _buttons.Clear();
    }

    protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is TalentViewModel talentViewModel)
        {
            switch (e.PropertyName)
            {
                case nameof(talentViewModel.CurrentTalentPointsValue):
                    if (talentViewModel.CurrentTalentPointsValue.TryGetValue(_statType, out int currentCountValue))
                    {
                        _currentTalent[_statType] = currentCountValue;
                        _currentTalentPointText.SetText(currentCountValue.ToString());
                        SetCurrentTalentPoint(_statType);
                    }
                    break;
                case nameof(talentViewModel.MaxTalentPoints):
                    if (talentViewModel.MaxTalentPoints.TryGetValue(_statType, out int maxCountValue))
                    {
                        _maxTalent[_statType] = maxCountValue;
                        _maxTalentPointsText.SetText(_maxTalent[_statType].ToString());
                    }
                    break;
            }
        }
    }

    protected void SetCurrentTalentPoint(TalentStatType statType)
    {
        if (_statType == statType)
        {
            if (_currentTalent[_statType] >= _maxTalent[_statType])
            {
                _buttons[_statType].interactable = false;
            }
            else { _buttons[_statType].interactable = true; }
        }
    }
}
