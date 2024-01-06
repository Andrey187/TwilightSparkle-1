using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerAbility : BaseAbilities
{
    private FlameThrower _flameThrower;
    private FireDoTEffect _fireDotEffect;
    private float _fireTimer;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    private List<IEnemy> enemiesToRemove = new List<IEnemy>();
    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;

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
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _flameThrower.CurrentAbility = _flameThrower;
        _flameThrower.DoTEffect = _fireDotEffect;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        EnemyEventManager.Instance.ObjectDie -= HandleEnemyDied;
    }

    protected override void Run()
    {
        _fireTimer += Time.deltaTime;

        if (_fireTimer >= _fireInterval && !_gamePause.IsPaused)
        {
            foreach (IEnemy enemy in enemiesInRange)
            {
                // Проверьте, что враг все еще активен и находится на сцене
                if (enemy != null)
                {
                    _setDamageIEnemy?.Invoke(enemy, _flameThrower.Damage, _flameThrower.CurrentAbility, _flameThrower.DoTEffect);
                }
            }

            _fireTimer = 0f;
        }

        // Удаление умерших врагов из списка
        foreach (IEnemy enemyToRemove in enemiesToRemove)
        {
            enemiesInRange.Remove(enemyToRemove);
        }
        enemiesToRemove.Clear(); // Очистщение временного списка после удаления

        if(LifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IEnemy hitEnemy) && !enemiesInRange.Contains(hitEnemy))
        {
            enemiesInRange.Add(hitEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IEnemy hitEnemy) && enemiesInRange.Contains(hitEnemy))
        {
            enemiesInRange.Remove(hitEnemy);
        }
    }

    private void HandleEnemyDied(GameObject enemy)
    {
        if (enemy.gameObject.TryGetComponent(out IEnemy hitEnemy) && enemiesInRange.Contains(hitEnemy))
        {
            enemiesToRemove.Add(hitEnemy);
        }
    }
}
