using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbilitiesOnButtonClick : MonoCache
{
    [SerializeField] private AbilityData _abilities;
    [SerializeField] private float _fireIntervalDecrease;
    [SerializeField] private int _damageIncrease;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        _abilities.Damage += _damageIncrease;
        _abilities.FireInterval -= _fireIntervalDecrease;
    }
}
