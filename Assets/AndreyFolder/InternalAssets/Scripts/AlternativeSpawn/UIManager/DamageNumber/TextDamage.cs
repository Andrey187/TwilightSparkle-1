using System;
using UnityEngine;

public abstract class TextDamage<T> : MonoCache, IDamageNumber
{
    [SerializeField] protected float _lifeTime = 1f;
    [SerializeField] protected float _moveSpeed = 2f;
    
    public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

}
