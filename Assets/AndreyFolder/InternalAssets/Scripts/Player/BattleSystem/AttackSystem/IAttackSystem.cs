using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackSystem
{
    Transform StartAttackPoint { get; set; }
    Transform EndAttackPoint { get; set; }

    List<BaseAbilities> AttackAbilitiesList { get; set; }
    public void SetCreatePrefabAbility(Action<BaseAbilities> createPrefabAbility);

    public void AddAttack(BaseAbilities ability);
}

