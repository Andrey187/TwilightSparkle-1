using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using DamageNumber;

[RequireComponent(typeof(MeshFilter), typeof(Mesh), typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoCache
{
    [SerializeField] protected internal EnemyType _enemyType;
    [SerializeField] protected internal int _currentHealth;
    [SerializeField] protected float _pushbackDistance = 0.2f;
    [SerializeField] protected float _pushbackDuration = 0.5f;
    protected HealthBarController _healthBarController;
    protected internal float _xpChangeTimer = 0f;
    protected bool _shouldIncrementXPTimer = true;
    protected bool _isBeingPushed = false;
    protected Vector3 _pushbackDirection;
    protected Coroutine _pushbackCoroutine;
    protected Rigidbody _rigidbody;
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected NavMeshAgent _navMeshAgent;

    protected void Awake()
    {
        _rigidbody = Get<Rigidbody>();
        _healthBarController = Get<HealthBarController>();
        _meshFilter = Get<MeshFilter>();
        _meshRenderer = Get<MeshRenderer>();
        _navMeshAgent = Get<NavMeshAgent>();
        _meshFilter.mesh = _enemyType.Mesh;
        _meshRenderer.material = _enemyType.Material;
        NavMeshParams();
    }

    protected override void OnEnabled()
    {
        if (_enemyType != null)
        {
            _enemyType.SetCurrentHealthToMax();
            _currentHealth = _enemyType.CurrentHealth;
        }
    }

    protected override void OnDisabled()
    {

        ResetXPTimer();
        SetShouldIncrementXPTimer(false);
    }

    protected internal void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    protected override void Run()
    {
        if (_shouldIncrementXPTimer)
        {
            _xpChangeTimer += Time.deltaTime;

            if (_xpChangeTimer >= 7f)
            {
                ReturnToPool();
            }
        }
    }

    protected virtual void TakeDamage(BaseEnemy enemy,int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        if (!gameObject.activeSelf)
        {
            return; // don't apply damage or show damage numbers if the game object is not active
        }

        if (enemy == this)
        {
            _currentHealth = ability.ApplyDamage(_currentHealth, damageAmount);
            _healthBarController.SetCurrentHealth(_currentHealth);
            _xpChangeTimer = 0f;
            if (enemy != null && damageAmount > 0 && _currentHealth > 0)
            {
                KnockBack();
                DamageNumberPool.Instance.Initialize(damageAmount, enemy.transform, ability);

                if (ability.HasDoT)
                {
                    StartCoroutine(PeriodicDamageCoroutine(
                        doTEffect,
                        enemy.transform,
                        doTEffect.Duration,
                        doTEffect.TickInterval,
                        damageAmount));
                }
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    protected IEnumerator PeriodicDamageCoroutine(IDoTEffect doTEffect,
        Transform target, float duration, float interval, int amount)
    {
        float timeLeft = duration;
        int periodicDamageAmount = doTEffect.ApplyDoT(_currentHealth, amount);

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(interval);
            _currentHealth -= periodicDamageAmount;
            if (_currentHealth <= 0)
            {
                Die();
                yield break; // exit the coroutine if the enemy is dead
            }

            DamageDoTNumberPool.Instance.Initialize(periodicDamageAmount, target.transform, doTEffect);
            _healthBarController.SetCurrentHealth(_currentHealth);
            timeLeft -= interval;
        }
    }

    protected void KnockBack()
    {
        // Start a coroutine to move the character towards the destination
        if (gameObject.activeSelf)
        {
            if (!_isBeingPushed)
            {
                _pushbackDirection = -transform.forward;
                _pushbackCoroutine = StartCoroutine(MoveToDestination());
            }
        }
    }

    protected IEnumerator MoveToDestination()
    {
        _isBeingPushed = true;
        float timer = 0f;

        while (timer < _pushbackDuration)
        {
            float distance = Mathf.Lerp(0f, _pushbackDistance, timer / _pushbackDuration);
            transform.position += _pushbackDirection * distance;

            timer += Time.deltaTime;
            yield return null;
        }

        _isBeingPushed = false;
    }

    public void ResetXPTimer()
    {
        _xpChangeTimer = 0f;
    }

    public void SetShouldIncrementXPTimer(bool shouldIncrement)
    {
        _shouldIncrementXPTimer = shouldIncrement;
    }

    public void Die()
    {
        ReturnToPool();
        LevelUpSystem.Instance.AddExperience(_enemyType._type, _enemyType);
    }

    private void ReturnToPool()
    {
        PoolObject<BaseEnemy>.Instance.ReturnObject(this);

        Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
        setObjectActive?.Invoke(gameObject, false);

        Action<GameObject> objectReturnToPool = EventManager.Instance.DestroyedObject;
        objectReturnToPool?.Invoke(gameObject);
    }

    private void NavMeshParams()
    {
        _navMeshAgent.baseOffset = 0.5f;
        _navMeshAgent.speed = _enemyType.Speed;
        _navMeshAgent.radius = 0.5f;
        _navMeshAgent.height = 0.5f;
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
}
