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
    [SerializeField] protected LayerMask _layerMask;
    protected abstract event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected virtual internal event Action<BaseAbilities> SetDie;
    protected Rigidbody _thisRb;
    
    protected internal virtual Vector3 TargetPoint { get; set; }
    protected internal virtual int AreaRadius { get; set; }
    protected internal virtual bool HasTargetPoint => false;

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

    protected internal void SetLifeTime()
    {
        _lifeTime = _baseLifeTime;
    }

    protected internal IEnumerator DecreaseLifeTime()
    {
        while (_lifeTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _lifeTime -= 1;
        }
    }

    protected internal virtual void MoveWithPhysics(Transform endPoint, Transform startPoint)
    {
        Vector3 direction = endPoint.position - startPoint.position;
        direction.Normalize();

        _thisRb.velocity = direction * _speed;
    }
}
