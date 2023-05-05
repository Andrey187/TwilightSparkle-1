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
    protected TalentViewModel _talentViewModel;

    public bool CanBeUpgraded => _currentTalentPointsValue != _maxTalentsPointCount;

    protected virtual void Start()
    {
        var talentSystem = _talentSystem;
        _talentViewModel = new TalentViewModel(talentSystem);

        _button.onClick.AddListener(() => _talentViewModel.OnButtonClick(_buttonTextValue));
        _talentViewModel.PropertyChanged += HandlePropertyChanged;
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_talentViewModel.TalentPoints))
        {
            if (CanBeUpgraded)
            {
                if (_currentTalentPointsValue < _maxTalentsPointCount) // check if max talent points has been reached
                {
                    _currentTalentPointsValue++;
                    _currentTalentPointText.SetText(_currentTalentPointsValue.ToString());
                    Debug.Log(CanBeUpgraded);
                }
            }
        }
    }
}
