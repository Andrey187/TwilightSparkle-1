using System;
using UnityEngine;
using UnityEngine.AI;
using DamageNumber;
using FSG.MeshAnimator;
using Zenject;

public abstract class BaseEnemy : MonoCache
{
    [SerializeField] protected internal EnemyData EnemyType;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected internal MeshAnimator meshAnimator;
    [Inject] protected ILevelUpSystem _ilevelUpSystem;
    protected IKnockback _iknockback;
    protected ITimedDisabler ItimedDisabler;
    protected HealthBarController _healthBarController;
   
    protected Rigidbody _rigidbody;
    protected internal NavMeshAgent _navMeshAgent;
    protected internal EnemyState CurrentState;
    protected float _currentSpeed;
    

    protected void Awake()
    {
        _rigidbody = Get<Rigidbody>();
        _healthBarController = Get<HealthBarController>();
        _navMeshAgent = Get<NavMeshAgent>();
        _iknockback = Get<IKnockback>();
        ItimedDisabler = Get<ITimedDisabler>();
        ItimedDisabler.OnTimerElapsed += ReturnToPool;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    protected override void OnEnabled()
    {
        if (EnemyType != null)
        {
            _navMeshAgent.enabled = true;
            NavMeshParams();
            EnemyType.SetCurrentHealthToMax();
            _currentHealth = EnemyType.CurrentHealth;
            _maxHealth = EnemyType.MaxHealth;
            transform.rotation = Quaternion.identity;
        }
    }
    private void UnsubscribeEvents()
    {
        ItimedDisabler.OnTimerElapsed -= ReturnToPool;
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

    protected virtual void TakeDamage(BaseEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        if (!gameObject.activeSelf)
        {
            return; // don't apply damage or show damage numbers if the game object is not active
        }

        if (enemy == this)
        {
            CurrentHealth = ability.ApplyDamage(CurrentHealth, damageAmount); //наносим урон

            ItimedDisabler.Timer = 0f;

            if (enemy != null && damageAmount > 0 && CurrentHealth > 0)
            {
                if (CurrentHealth != 0)
                {
                    ChangeState(new TakingDamageState()); //смена анимации при получении урона
                }

                _iknockback.KnockBack(gameObject, transform); //отталкивание при получении урона

                DamageNumberPool.Instance.InitializeGetObjectFromPool(damageAmount, gameObject.transform, ability); //вызов текста урона

                if (ability.HasDoT)
                {
                    DoTManager.Instance.RegisterDoT(doTEffect, this, damageAmount); //если абилка имеет HasDoT, то вызывается текст дот урона
                }

            }
            if (_currentHealth <= 0)
            {
                DoTManager.Instance.UnregisterDoTsForTarget(this);
                Die();
            }
        }
    }

    protected internal void Die()
    {
        _navMeshAgent.enabled = false;
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.EnemyDie);

        Action<GameObject> deathParticleInvoke = ParticleEventManager.Instance.DeathParticle;
        deathParticleInvoke?.Invoke(gameObject);

        ReturnToPool();
        DropEventManager.Instance.DropsCreated(gameObject);
        EnemyEventManager.Instance.DieObject(gameObject);
        _ilevelUpSystem.AddExperience(EnemyType._type, EnemyType);
    }

    protected void ReturnToPool()
    {
        Action<GameObject> objectReturnToPool = EnemyEventManager.Instance.DestroyedObject;
        objectReturnToPool?.Invoke(gameObject);

        Action<GameObject, bool> setObjectActive = EnemyEventManager.Instance.SetObjectActive;
        setObjectActive?.Invoke(gameObject, false);
    }

    protected void NavMeshParams()
    {
        _navMeshAgent.speed = EnemyType.Speed;
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
}
