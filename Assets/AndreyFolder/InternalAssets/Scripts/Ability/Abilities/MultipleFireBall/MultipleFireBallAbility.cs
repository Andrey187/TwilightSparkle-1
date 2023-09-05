using System;
using UnityEngine;

public class MultipleFireBallAbility : BaseAbilities
{
    [SerializeField] private int _countProjectile;
    private MultipleFireBall _multipleFireBall;
    private FireDoTEffect _fireDotEffect;
    
    private float _lastExecutionTime;
    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected internal override event Action<BaseAbilities> SetDie;
    protected override float LastExecutionTime { get => _lastExecutionTime; set => _lastExecutionTime = value; }
    public override int AlternativeCountAbilities { get => _countProjectile; set => _countProjectile = value; }

    private void Start()
    {
        _multipleFireBall = MultipleFireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _multipleFireBall.CurrentAbility = _multipleFireBall;
        _multipleFireBall.DoTEffect = _fireDotEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Profiling.Profiler.BeginSample("MyltipleBall");

        if (other.TryGetComponent(out IEnemy enemy))
        {
            // Apply damage to the enemy
            _setDamageIEnemy?.Invoke(enemy, _multipleFireBall.Damage, _multipleFireBall.CurrentAbility, _multipleFireBall.DoTEffect);
        }

        if ((_layerMaskForDie.value & (1 << other.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }
}
