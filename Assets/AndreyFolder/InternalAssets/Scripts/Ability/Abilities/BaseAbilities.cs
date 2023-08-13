using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAbilities : MonoCache
{
    [SerializeField] protected AbilityData abilityData;
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected float _fireInterval;
    [SerializeField] protected float _baseLifeTime;
    [SerializeField] protected float _lifeTime;
    [SerializeField] protected LayerMask _layerMaskForDie;
    [SerializeField] protected float damageRadius = 2.0f;

    protected virtual float LastExecutionTime { get; set; }

    protected abstract event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected virtual internal event Action<BaseAbilities> SetDie;
    protected virtual internal event Action SetCreate;
    protected Rigidbody _thisRb;

    protected internal virtual Vector3 TargetPoint { get; set; }
    protected internal virtual int AreaRadius { get; set; }
    protected internal virtual bool HasTargetPoint => false;

    protected internal virtual int AlternativeCountAbilities { get; set; }

    protected internal float AngleStep { get; set; } = 0f;
    protected internal float FireInterval { get => _fireInterval; set => _fireInterval = value; }
    protected internal float LifeTime { get => _lifeTime; set => _lifeTime = value; }

    protected override void OnEnabled()
    {
        _fireInterval = abilityData.FireInterval;
        StartCoroutine(DecreaseLifeTime());
    }

    protected override void OnDisabled()
    {
        StopCoroutine(DecreaseLifeTime());
        SetLifeTime();
    }

    protected void SetLifeTime()
    {
        LifeTime = _baseLifeTime;
    }

    protected IEnumerator DecreaseLifeTime()
    {
        while (LifeTime > 0)
        {
            yield return new WaitForSeconds(1f);
            LifeTime -= 1;
        }
    }

    protected internal virtual void MoveWithPhysics(Transform endPoint, Transform startPoint)
    {
        Vector3 direction = endPoint.position - startPoint.position;
        direction.Normalize();

        _thisRb.velocity = direction * _speed;
    }
    protected internal virtual void RaiseSetCreateEvent() { }
    protected internal virtual void CalculateAndIncrementAngle(float countOfProjectiles) { }
    protected internal virtual void CalculateAlternativeMovePosition() { }

    protected internal virtual void AlternativeMove() { }

    protected virtual void PerformDamageCheck() { }
}
