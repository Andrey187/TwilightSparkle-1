using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using DamageNumber;
using FSG.MeshAnimator;

public abstract class BaseEnemy : MonoCache
{
    [SerializeField] protected internal EnemyData EnemyType;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected float _pushbackDistance = 0.2f;
    [SerializeField] protected float _pushbackDuration = 0.5f;
    protected HealthBarController _healthBarController;
    protected internal EnemyState CurrentState;
    protected internal float HpChangeTimer = 0f;
    protected bool _shouldIncrementHPTimer = true;
    protected bool _isBeingPushed = false;
    protected Vector3 _pushbackDirection;
    protected Coroutine _pushbackCoroutine;
    protected Rigidbody _rigidbody;
    protected NavMeshAgent _navMeshAgent;
    protected internal SkinnedMeshRenderer _skinnedMesh;
    [SerializeField] protected internal Renderer _renderer;
    [SerializeField]
    protected internal MeshAnimator meshAnimator;

    protected void Awake()
    {
        _rigidbody = Get<Rigidbody>();
        _healthBarController = Get<HealthBarController>();
        _navMeshAgent = Get<NavMeshAgent>();

        NavMeshParams();
    }

    protected override void OnEnabled()
    {
        if (EnemyType != null)
        {
            EnemyType.SetCurrentHealthToMax();
            _currentHealth = EnemyType.CurrentHealth;
            _maxHealth = EnemyType.MaxHealth;
            _renderer.enabled = false;
            transform.rotation = Quaternion.identity;
            ResetHPTimer();
            SetShouldIncrementHPTimer(true);
        }
    }

    protected override void OnDisabled()
    {
        ResetHPTimer();
        SetShouldIncrementHPTimer(false);
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Max(value, 0);
            _healthBarController.SetCurrentHealth(_currentHealth);
        }
    }

    protected internal Vector3 OnCreate(Vector3 position)
    {
        transform.position = position;
        return transform.position;
    }

    protected internal void ChangeState(EnemyState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit(this);
        }

        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.Enter(this);
        }
    }

    protected override void Run()
    {
        if (_shouldIncrementHPTimer)
        {
            HpChangeTimer += Time.deltaTime;

            if (HpChangeTimer >= 8f)
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
            CurrentHealth = ability.ApplyDamage(CurrentHealth, damageAmount);
            HpChangeTimer = 0f;
            if (enemy != null && damageAmount > 0 && CurrentHealth > 0)
            {
                ChangeState(new TakingDamageState());
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
        int periodicDamageAmount = doTEffect.ApplyDoT(CurrentHealth, amount);

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(interval);
            CurrentHealth -= periodicDamageAmount;
            if (CurrentHealth <= 0)
            {
                Die();
                yield break; // exit the coroutine if the enemy is dead
            }

            DamageDoTNumberPool.Instance.Initialize(periodicDamageAmount, target.transform, doTEffect);
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
            if (Time.timeScale > 0f)
            {
                float distance = Mathf.Lerp(0f, _pushbackDistance, timer / _pushbackDuration);
                transform.position += _pushbackDirection * distance;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        _isBeingPushed = false;
    }

    public void ResetHPTimer()
    {
        HpChangeTimer = 0f;
    }

    public void SetShouldIncrementHPTimer(bool shouldIncrement)
    {
        _shouldIncrementHPTimer = shouldIncrement;
    }

    public void Die()
    {
        ReturnToPool();
        DropEventManager.Instance.DropsCreated(gameObject);
        LevelUpSystem.Instance.AddExperience(EnemyType._type, EnemyType);
    }

    private void ReturnToPool()
    {
        Action<GameObject, bool> setObjectActive = EnemyEventManager.Instance.SetObjectActive;
        setObjectActive?.Invoke(gameObject, false);

        Action<GameObject> objectReturnToPool = EnemyEventManager.Instance.DestroyedObject;
        objectReturnToPool?.Invoke(gameObject);
    }

    private void NavMeshParams()
    {
        _navMeshAgent.speed = EnemyType.Speed;
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
}
