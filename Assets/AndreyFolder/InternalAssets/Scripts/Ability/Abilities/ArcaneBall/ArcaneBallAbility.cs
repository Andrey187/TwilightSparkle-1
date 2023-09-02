using System;
using UnityEngine;

public class ArcaneBallAbility : BaseAbilities
{
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;
    private ArcaneBall _arcaneBall;

    protected override void OnEnabled()
    {
        base.OnEnabled();
    }

    private void Start()
    {
        _arcaneBall = new ArcaneBall();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _arcaneBall.CurrentAbility = _arcaneBall;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the fireball is an enemy
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null )
        {
            // If so, damage the enemy and destroy the fireball
            _setDamage?.Invoke(enemy, _arcaneBall.Damage, _arcaneBall.CurrentAbility, _arcaneBall.DoTEffect);
            
        }
        if ((_layerMaskForDie.value & (1 << other.transform.gameObject.layer)) > 0)
        {   
            SetDie?.Invoke(this);
        }
    }
}
