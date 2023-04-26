using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRunner : MonoCache
{
    private IAbility _currentAbility;

    private IDoTEffect _doTEffect;

    public IAbility CurrentAbility 
    {   get => _currentAbility; 
        set => _currentAbility = value; 
    }

    public IDoTEffect DoTEffect
    {
        get => _doTEffect;
        set => _doTEffect = value;
    }

    protected override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action<int, IAbility, IDoTEffect> setObjectActive = EventManager.Instance.AbillityDamage;
            setObjectActive?.Invoke(CurrentAbility.Damage, CurrentAbility, DoTEffect);
        }
    }
}

