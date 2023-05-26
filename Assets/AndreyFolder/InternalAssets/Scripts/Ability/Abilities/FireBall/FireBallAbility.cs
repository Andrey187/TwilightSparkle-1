using System;
using UnityEngine;

public class FireBallAbility: BaseAbilities
{
    private FireBall _fireBall;
    private FireDoTEffect _fireDotEffect;
    private event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    private void Start()
    {
        _fireBall = new FireBall();
        _fireDotEffect = new FireDoTEffect();
        _setDamage = EventManager.Instance.AbillityDamage;
        _fireBall.CurrentAbility = _fireBall;
        _fireBall.DoTEffect = _fireDotEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the fireball is an enemy
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            // If so, damage the enemy and destroy the fireball
            _setDamage?.Invoke(enemy,_fireBall.Damage, _fireBall.CurrentAbility, _fireBall.DoTEffect);
            SetDie?.Invoke(this);
        }
    }
}