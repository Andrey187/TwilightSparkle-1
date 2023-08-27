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
    [SerializeField] protected internal Renderer _renderer;
    [SerializeField] protected internal MeshAnimator meshAnimator;
    [Inject] protected ILevelUpSystem _ilevelUpSystem;
    protected IKnockback _iknockback;
    protected internal ITimedDisabler ItimedDisabler;
    protected HealthBarController _healthBarController;
   
    protected Rigidbody _rigidbody;
    protected internal NavMeshAgent NavMeshAgent;
    protected internal EnemyState CurrentState;
    protected float _currentSpeed;
    

    protected void Awake()
    {
        _rigidbody = Get<Rigidbody>();
        _healthBarController = Get<HealthBarController>();
        NavMeshAgent = Get<NavMeshAgent>();
        _iknockback = Get<IKnockback>();

        ItimedDisabler = Get<ITimedDisabler>();
       
        ItimedDisabler.OnTimerElapsed += ReturnToPool;
        
    }

    protected override void OnEnabled()
    {
        if (EnemyType != null)
        {
            NavMeshAgent.enabled = true;
            NavMeshParams();
            EnemyType.SetCurrentHealthToMax();
            _currentHealth = EnemyType.CurrentHealth;
            _maxHealth = EnemyType.MaxHealth;
            _renderer.enabled = false;
            transform.rotation = Quaternion.identity;
        }
    }

    protected void OnDestroy()
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

    protected virtual void TakeDamage(BaseEnemy enemy,int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        if (!gameObject.activeSelf)
        {
            return; // don't apply damage or show damage numbers if the game object is not active
        }

        if (enemy == this)
        {
            CurrentHealth = ability.ApplyDamage(CurrentHealth, damageAmount);
            ItimedDisabler.Timer = 0f;
            if (enemy != null && damageAmount > 0 && CurrentHealth > 0)
            {
                if (CurrentHealth != 0)
                {
                    ChangeState(new TakingDamageState());
                }
                _iknockback.KnockBack(gameObject, transform);
                DamageNumberPool.Instance.Initialize(damageAmount, enemy.transform, ability);

                if (ability.HasDoT)
                {
                    DoTManager.Instance.RegisterDoT(doTEffect, enemy, damageAmount);
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
        NavMeshAgent.enabled = false;
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
        NavMeshAgent.speed = EnemyType.Speed;
        NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
}
