using System;
using UnityEngine;

public class ArcaneBallAbility : BaseAbilities
{
    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected internal override event Action<BaseAbilities> SetDie;
    private ArcaneBall _arcaneBall;

    private void Start()
    {
        _arcaneBall = new ArcaneBall();
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _arcaneBall.CurrentAbility = _arcaneBall;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the fireball is an enemy
        if (other.TryGetComponent(out IEnemy enemy))
        {
            // If so, damage the enemy and destroy the fireball
            _setDamageIEnemy?.Invoke(enemy, _arcaneBall.Damage, _arcaneBall.CurrentAbility, _arcaneBall.DoTEffect);
            
        }
        if ((_layerMaskForDie.value & (1 << other.transform.gameObject.layer)) > 0)
        {   
            SetDie?.Invoke(this);
        }
    }
}
