using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AddingAbilityOnButtonClick : MonoCache
{
    [SerializeField] private BaseAbilities _abilities;
    [Inject] private IAttackSystem _attackSystem;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Trigger the AddAttackScript method of the AttackScript class
        _attackSystem.AddAttack(_abilities);
        _abilities.RaiseSetCreateEvent();
    }
}
