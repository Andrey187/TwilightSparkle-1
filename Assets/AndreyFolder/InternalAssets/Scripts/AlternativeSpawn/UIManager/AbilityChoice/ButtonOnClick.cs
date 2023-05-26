using AbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClick : MonoCache
{
    [SerializeField] private AttackSystem _attackSystem;
    [SerializeField] private BaseAbilities _abilities;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _attackSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackSystem>();
        _button.onClick.AddListener(OnButtonClick);

    }

    private void OnButtonClick()
    {
        // Trigger the AddAttackScript method of the AttackScript class
        _attackSystem.AddAttackScript(_abilities);
    }
}
