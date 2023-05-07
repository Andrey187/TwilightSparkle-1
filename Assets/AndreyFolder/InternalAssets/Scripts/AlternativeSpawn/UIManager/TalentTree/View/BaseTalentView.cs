using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTalentView : MonoBehaviour
{
    [SerializeField] protected TalentSystem _talentSystem;
    [SerializeField] protected TextMeshProUGUI _nameText;
    [SerializeField] protected Button _button;
    [SerializeField] protected TextMeshProUGUI _buttonText;
    [SerializeField] protected TextMeshProUGUI _currentTalentPointText;
    [SerializeField] protected TextMeshProUGUI _maxTalentPointsText;
    [SerializeField] protected int _currentTalentPointsValue;
    [SerializeField] protected int _maxTalentsPointCount;
    [SerializeField] protected int _buttonTextValue;
    [SerializeField] protected TalentStatType _statType;
    protected TalentViewModel _talentViewModel;

    protected virtual void Start()
    {
        var talentSystem = _talentSystem;
        _talentViewModel = new TalentViewModel(talentSystem);

        _button.onClick.AddListener(() => _talentViewModel.OnButtonClick(_statType,_buttonTextValue));
        _talentViewModel.PropertyChanged += HandlePropertyChanged;
        _talentViewModel.SetMaxTalentPoints(_maxTalentsPointCount);
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_talentViewModel.CurrentTalentPointsValue))
        {
            _currentTalentPointsValue = _talentViewModel.CurrentTalentPointsValue;
            _currentTalentPointText.SetText(_talentViewModel.CurrentTalentPointsValue.ToString());
            if(_currentTalentPointsValue >= _maxTalentsPointCount)
            {
                _button.enabled = false;
            }
        }
    }
}
