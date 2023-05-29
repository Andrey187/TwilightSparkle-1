using System;
using UnityEngine;

public class FrostBallAbility : BaseAbilities
{
    private FrostBall _frostBall;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    private void Start()
    {
        _frostBall = new FrostBall();
        _setDamage = EventManager.Instance.AbillityDamage;
        _frostBall.CurrentAbility = _frostBall;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the fireball is an enemy
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            // If so, damage the enemy and destroy the fireball
            _setDamage?.Invoke(enemy, _frostBall.Damage, _frostBall.CurrentAbility, _frostBall.DoTEffect);
        }

        // Check if the collided object has the specified mask layer
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
    }
}
