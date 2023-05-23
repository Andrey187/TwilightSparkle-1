using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAbilities : MonoCache
{
    [SerializeField] protected Transform _startPoint;
    [SerializeField] protected Transform _endPoint;
    [SerializeField] protected float _speed = 0.5f;
    [SerializeField] protected float _fireInterval;
    [SerializeField] protected float _baseLifeTime;
    [SerializeField] protected float _lifeTime;
    protected internal virtual event Action<BaseAbilities> SetDie;
    protected Rigidbody _thisRb;
    
    protected internal Transform StartPoint { get => _startPoint; set => _startPoint = value; }
    protected internal Transform EndPoint { get => _endPoint; set => _endPoint = value; }
    protected internal float FireInterval { get => _fireInterval; set => _fireInterval = value; }
    protected internal float LifeTime { get => _lifeTime; set => _lifeTime = value; }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        StartCoroutine(DecreaseLifeTime());
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
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

    protected internal void MoveWithPhysics() 
    {
        Vector3 direction = _endPoint.position - _startPoint.position;
        direction.Normalize();

        _thisRb.velocity = direction * _speed;
    }
}
