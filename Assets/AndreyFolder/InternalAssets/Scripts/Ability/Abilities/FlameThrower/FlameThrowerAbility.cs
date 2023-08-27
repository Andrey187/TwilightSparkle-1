using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerAbility : BaseAbilities
{
    [SerializeField] private LayerMask _enemyLayer;
  
    private FlameThrower _flameThrower;
    private FireDoTEffect _fireDotEffect;
    private float _fireTimer;
    private List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    private List<BaseEnemy> enemiesToRemove = new List<BaseEnemy>();
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;

    protected override void OnEnabled()
    {
        base.OnEnabled();
        enemiesInRange.Clear();
    }

    private void Start()
    {
        _flameThrower = FlameThrower.Instance;
        _fireDotEffect = new FireDoTEffect();
        EnemyEventManager.Instance.ObjectDie += HandleEnemyDied;
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _flameThrower.CurrentAbility = _flameThrower;
        _flameThrower.DoTEffect = _fireDotEffect;
    }

    protected override void OnDisabled()
    {
        EnemyEventManager.Instance.ObjectDie -= HandleEnemyDied;
    }

    protected override void Run()
    {
        _fireTimer += Time.deltaTime;

        if (_fireTimer >= _fireInterval)
        {
            foreach (BaseEnemy enemy in enemiesInRange)
            {
                // Проверьте, что враг все еще активен и находится на сцене
                if (enemy != null && enemy.isActiveAndEnabled)
                {
                    _setDamage?.Invoke(enemy, _flameThrower.Damage, _flameThrower.CurrentAbility, _flameThrower.DoTEffect);
                }
            }

            _fireTimer = 0f;
        }

        // Удаление умерших врагов из списка
        foreach (BaseEnemy enemyToRemove in enemiesToRemove)
        {
            enemiesInRange.Remove(enemyToRemove);
        }
        enemiesToRemove.Clear(); // Очистщение временного списка после удаления
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

    private void HandleEnemyDied(GameObject enemy)
    {
        BaseEnemy _enemy = enemy.GetComponent<BaseEnemy>();
        if (enemiesInRange.Contains(_enemy))
        {
            enemiesToRemove.Add(_enemy);
        }
    }
}
