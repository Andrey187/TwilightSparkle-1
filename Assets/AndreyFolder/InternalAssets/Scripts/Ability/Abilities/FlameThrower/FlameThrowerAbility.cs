using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerAbility : BaseAbilities
{
    [SerializeField] private LayerMask _enemyLayer;
  
    private FlameThrower _flameThrower;
    private FireDoTEffect _fireDotEffect;
    private float _fireTimer;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;
    private List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        enemiesInRange.Clear();
    }

    private void Start()
    {
        _flameThrower = FlameThrower.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _flameThrower.CurrentAbility = _flameThrower;
        _flameThrower.DoTEffect = _fireDotEffect;
    }

    protected override void Run()
    {
        _fireTimer += Time.deltaTime;

        if (_fireTimer >= _fireInterval)
        {
            foreach (BaseEnemy enemy in enemiesInRange)
            {
                _setDamage?.Invoke(enemy, _flameThrower.Damage, _flameThrower.CurrentAbility, _flameThrower.DoTEffect);
            }

            _fireTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }



    //private void OnTriggerStay(Collider other)
    //{
    //    // Assuming enemies have a BaseEnemy component
    //    BaseEnemy enemy = other.GetComponent<BaseEnemy>();

    //    if (enemy != null && _fireTimer >= _fireInterval)
    //    {
    //        // Apply damage to the enemy
    //        _setDamage?.Invoke(enemy, _flameThrower.Damage, _flameThrower.CurrentAbility, _flameThrower.DoTEffect);
    //        _fireTimer = 0f;
    //        Debug.Log("Damage");
    //    }
    //}
}
