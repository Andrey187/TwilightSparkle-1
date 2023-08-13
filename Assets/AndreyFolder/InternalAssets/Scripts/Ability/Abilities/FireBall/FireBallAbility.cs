using System;
using UnityEngine;

public class FireBallAbility: BaseAbilities
{
    private FireBall _fireBall;
    private FireDoTEffect _fireDotEffect;
    private Collider[] _hitColliders;
    [SerializeField] private LayerMask _enemyLayer;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    protected override internal event Action SetCreate;

    private float _lastExecutionTime;
    protected override float LastExecutionTime { get=> _lastExecutionTime; set=> _lastExecutionTime = value; }

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.FireBall);
    }

    private void Start()
    {
        _fireBall = FireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _fireBall.CurrentAbility = _fireBall;
        _fireBall.DoTEffect = _fireDotEffect;
    }

    protected internal override void RaiseSetCreateEvent()
    {
        SetCreate?.Invoke();
    }

    protected override void Run()
    {
        UnityEngine.Profiling.Profiler.BeginSample("FireBall");
        float currentTime = Time.time;
        if (currentTime - _lastExecutionTime >= 0.5f)
        {
            _lastExecutionTime = currentTime;
            PerformDamageCheck();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected override void PerformDamageCheck()
    {
        if (LifeTime <= 0)
        {
            // Clear the hitColliders array
            _hitColliders = new Collider[0];
            return; // Exit the method early without processing collisions
        }

        Vector3 currentPosition = transform.position;

        // Cast a sphere to detect enemies within the damage radius
        _hitColliders = Physics.OverlapSphere(currentPosition, damageRadius, _enemyLayer);

        foreach (Collider hitCollider in _hitColliders)
        {
            // Assuming enemies have a BaseEnemy component
            BaseEnemy enemy = hitCollider.GetComponent<BaseEnemy>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                _setDamage?.Invoke(enemy, _fireBall.Damage, _fireBall.CurrentAbility, _fireBall.DoTEffect);
            }

            if ((_layerMaskForDie.value & (1 << hitCollider.transform.gameObject.layer)) > 0)
            {
                // If the collided object has the specified layer, invoke SetDie
                SetDie?.Invoke(this);
            }
        }
    }
}