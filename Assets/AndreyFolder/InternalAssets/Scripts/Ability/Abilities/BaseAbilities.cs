using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAbilities : MonoCache, IMultipleProjectileCount
{
    [SerializeField] protected AbilityData abilityData;
    [SerializeField] protected float _fireInterval;
    [SerializeField] protected float _baseLifeTime;
    [SerializeField] protected float _lifeTime;
    [SerializeField] protected LayerMask _layerMaskForDie;
    [SerializeField] protected float damageRadius = 2.0f;
    [SerializeField] protected bool _isMultiple = false;
    [SerializeField] protected Sound.SoundEnum soundEnum;
    protected BaseEnemy _baseEnemy;
    protected abstract event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected internal virtual event Action<BaseAbilities> SetDie;

    protected virtual float LastExecutionTime { get; set; }
    protected internal Sound.SoundEnum SoundEnum { get => soundEnum; set => soundEnum = value; }
    protected internal float FireInterval { get => _fireInterval; set => _fireInterval = value; }
    protected internal float LifeTime { get => _lifeTime; set => _lifeTime = value; }
    protected internal bool IsMultiple { get => _isMultiple; set => _isMultiple = value; }
    public virtual int AlternativeCountAbilities { get; set; }

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

    protected internal virtual void RaiseSetCreateEvent() { }
   
    protected virtual void PerformDamageCheck() { }
}
