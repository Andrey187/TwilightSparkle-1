using System.Collections.Generic;
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
    [SerializeField] protected int _currentTalentPointsValue;
    [SerializeField] protected int _maxTalentsPointCount;
    protected TalentViewModel _talentViewModel;
    protected Dictionary<TalentStatType, Button> _buttons = new Dictionary<TalentStatType, Button>();
    protected Dictionary<TalentStatType, int> _currentTalent = new Dictionary<TalentStatType, int>();
    protected Dictionary<TalentStatType, int> _maxTalent = new Dictionary<TalentStatType, int>();

    protected virtual void Start()
    {
        _talentViewModel = new TalentViewModel(_talentSystem);
        _currentTalent.Add(_statType, _currentTalentPointsValue);
        _maxTalent.Add(_statType, _maxTalentsPointCount);

        _talentSystem.OnCurrentTalentPoint += SetCurrentTalentPoint;
        _talentViewModel.SetMaxTalentPoints(_maxTalent);
        _talentViewModel.SetCurrentTalentPoints(_currentTalent);

        _buttons.Add(_statType, _button);
        _buttons[_statType].onClick.AddListener(() => _talentViewModel.OnButtonClick(_statType, _buttonTextValue));
    }

    private void SetCurrentTalentPoint(TalentStatType statType, int value)
    {
        if (_statType == statType)
        {
            _currentTalent[_statType] = value;
            _currentTalentPointText.SetText(value.ToString());
            if (_currentTalent[_statType] >= _maxTalent[_statType])
            {
                _buttons[_statType].enabled = false;
            }
            Debug.Log(value);
        }
    }
}
