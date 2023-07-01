using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbilitiesOnButtonClick : MonoCache, INotifyPropertyChanged
{
    [SerializeField] private AbilityData _abilities;
    //[SerializeField] private float _fireIntervalDecrease;
    [SerializeField] private int _damageIncrease;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    public int Index;

    public event PropertyChangedEventHandler PropertyChanged;

    private AbilityViewModel _abilityViewModel;
    private Button _button;

    public int DamageIncrease
    {
        get { return _damageIncrease; }
        set
        {
            if (_damageIncrease != value)
            {
                _damageIncrease = value;
                OnPropertyChanged(nameof(DamageIncrease));
                _descriptionText.SetText(/*"Decrease " + _fireIntervalDecrease + " sec" + */"\n Increase " + DamageIncrease.ToString() + " damage");
            }
        }
    }

    
    private void Awake()
    {
        if (_abilityViewModel == null)
        {
            _abilityViewModel = new AbilityViewModel(_abilities, /*_fireIntervalDecrease,*/ DamageIncrease);
        }
        _button = Get<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    protected override void OnEnabled()
    {
        _descriptionText.SetText(/*"Decrease " + _fireIntervalDecrease + " sec" + */"\n Increase " + DamageIncrease.ToString() + " damage");
    }

    private void OnButtonClick()
    {
        _abilityViewModel.UpgradeAbility(DamageIncrease);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
