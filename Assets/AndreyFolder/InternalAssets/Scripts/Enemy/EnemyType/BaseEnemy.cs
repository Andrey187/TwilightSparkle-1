using System;
using UnityEngine;
using UnityEngine.AI;
using DamageNumber;
using FSG.MeshAnimator;
using Zenject;

public abstract class BaseEnemy : MonoCache, IEnemy
{
    [SerializeField] protected EnemyData EnemyType;
    protected int _currentHealth;
    protected int _maxHealth;
    protected float _currentSpeed;
    [Inject] protected ILevelUpSystem _ilevelUpSystem;
    protected IKnockback _iknockback;
    protected ITimedDisabler ItimedDisabler;

    [SerializeField] protected HealthBarController _healthBarController;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected MeshAnimator meshAnimator;
    [SerializeField] protected MeshRenderer _meshRenderer;
    protected internal EnemyState CurrentState;

    protected event Action<GameObject> _deathParticleDelegate;
    protected event Action<GameObject> _objectReturnToPoolDelegate;
    protected event Action<IEnemy, bool> _setObjectActiveDelegate;
   
    BaseEnemy IEnemy.BaseEnemy { get => this; }

    EnemyData IEnemy.EnemyType { get => EnemyType; }

    MeshAnimator IEnemy.MeshAnimator { get => meshAnimator; }

    HealthBarController IEnemy.HealthBarController { get => _healthBarController; }

    MeshRenderer IEnemy.MeshRenderer { get => _meshRenderer; }

    protected virtual void Awake()
    {
        _iknockback = Get<IKnockback>();
        ItimedDisabler = Get<ITimedDisabler>();
        ItimedDisabler.OnTimerElapsed += ReturnToPool;
        AbilityEventManager.Instance.TakeAbilityDamageIEnemy += TakeDamage;

        _deathParticleDelegate = ParticleEventManager.Instance.DeathParticle;
        _objectReturnToPoolDelegate = EnemyEventManager.Instance.DestroyedObject;
        _setObjectActiveDelegate = EnemyEventManager.Instance.SetObjectActive;

        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    protected override void OnEnabled()
    {
        if (EnemyType != null)
        {
            EnemyType.SetCurrentHealthToMax();
            _currentHealth = EnemyType.CurrentHealth;
            _maxHealth = EnemyType.MaxHealth;
            transform.rotation = Quaternion.identity;
        }
    }
    private void UnsubscribeEvents()
    {
        ItimedDisabler.OnTimerElapsed -= ReturnToPool;
        AbilityEventManager.Instance.TakeAbilityDamageIEnemy -= TakeDamage;
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

    protected virtual void TakeDamage(IEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        if (!gameObject.activeSelf)
        {
            return; // don't apply damage or show damage numbers if the game object is not active
        }
        
        if (enemy.BaseEnemy == this)
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
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.EnemyDie);

        _deathParticleDelegate?.Invoke(gameObject);

        ReturnToPool();
        DropEventManager.Instance.DropsCreated(gameObject);
        EnemyEventManager.Instance.DieObject(gameObject);
        _ilevelUpSystem.AddExperience(EnemyType._type, EnemyType);
    }

    protected internal void ReturnToPool()
    {
        _objectReturnToPoolDelegate?.Invoke(gameObject);
        _setObjectActiveDelegate?.Invoke(this, false);
    }
}
