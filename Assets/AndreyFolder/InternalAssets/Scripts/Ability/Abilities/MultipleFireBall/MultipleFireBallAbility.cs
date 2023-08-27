using System;
using UnityEngine;

public class MultipleFireBallAbility : BaseAbilities
{
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] private int _countProjectile;
    private MultipleFireBall _multipleFireBall;
    private FireDoTEffect _fireDotEffect;
    private Collider[] _hitColliders;
    
    private float _lastExecutionTime;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;
    protected override float LastExecutionTime { get => _lastExecutionTime; set => _lastExecutionTime = value; }
    public override int AlternativeCountAbilities { get => _countProjectile; set => _countProjectile = value; }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.FireBall);
    }

    private void Start()
    {
        _multipleFireBall = MultipleFireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _multipleFireBall.CurrentAbility = _multipleFireBall;
        _multipleFireBall.DoTEffect = _fireDotEffect;
    }

    protected override void Run()
    {
        UnityEngine.Profiling.Profiler.BeginSample("MyltipleBall");
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
                _setDamage?.Invoke(enemy, _multipleFireBall.Damage, _multipleFireBall.CurrentAbility, _multipleFireBall.DoTEffect);
            }

            if ((_layerMaskForDie.value & (1 << hitCollider.transform.gameObject.layer)) > 0)
            {
                // If the collided object has the specified layer, invoke SetDie
                SetDie?.Invoke(this);
            }
        }
    }
}
