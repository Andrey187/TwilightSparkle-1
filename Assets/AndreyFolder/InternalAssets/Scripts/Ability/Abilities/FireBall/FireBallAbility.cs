using System;
using UnityEngine;

public class FireBallAbility: BaseAbilities
{
    private FireBall _fireBall;
    private FireDoTEffect _fireDotEffect;
    private float _lastExecutionTime;
    protected internal override event Action<BaseAbilities> SetDie;

    protected internal event Action SetCreate;

    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected override float LastExecutionTime { get=> _lastExecutionTime; set=> _lastExecutionTime = value; }

    private void Start()
    {
        _fireBall = FireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _fireBall.CurrentAbility = _fireBall;
        _fireBall.DoTEffect = _fireDotEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Profiling.Profiler.BeginSample("FireBall Loop");

        if (other.TryGetComponent(out IEnemy enemy))
        {
            // Apply damage to the enemy
            _setDamageIEnemy?.Invoke(enemy, _fireBall.Damage, _fireBall.CurrentAbility, _fireBall.DoTEffect);
        }
        
        if ((_layerMaskForDie.value & (1 << other.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected internal override void RaiseSetCreateEvent()
    {
        SetCreate?.Invoke();
    }
}