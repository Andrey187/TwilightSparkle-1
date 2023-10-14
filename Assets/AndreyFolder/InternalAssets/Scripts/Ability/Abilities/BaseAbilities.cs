using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

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
    [Inject] protected IGamePause _gamePause;
    protected BaseEnemy _baseEnemy;
    protected abstract event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected internal virtual event Action<BaseAbilities> SetDie;
    protected CancellationTokenSource _cancellationTokenSource;

    protected virtual float LastExecutionTime { get; set; }
    protected internal Sound.SoundEnum SoundEnum { get => soundEnum; set => soundEnum = value; }
    protected internal float FireInterval { get => _fireInterval; set => _fireInterval = value; }
    protected internal float LifeTime { get => _lifeTime; set => _lifeTime = value; }
    protected internal bool IsMultiple { get => _isMultiple; set => _isMultiple = value; }
    public virtual int AlternativeCountAbilities { get; set; }

    protected async override void OnEnabled()
    {
        _fireInterval = abilityData.FireInterval;

        _cancellationTokenSource = new CancellationTokenSource();

        _ = DecreaseLifeTime(_cancellationTokenSource.Token);
        await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
    }

    protected override void OnDisabled()
    {
        _cancellationTokenSource?.Cancel();

        SetLifeTime();
    }

    protected void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }

    protected void SetLifeTime()
    {
        LifeTime = _baseLifeTime;
    }

    private async UniTask DecreaseLifeTime(CancellationToken cancellationToken)
    {
        while (LifeTime > 0)
        {
            if (!_gamePause.IsPaused)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await UniTask.Delay(TimeSpan.FromSeconds(1f), false, PlayerLoopTiming.Update, cancellationToken);
                LifeTime -= 1;
            }
            else
            {
                // ≈сли игра на паузе, просто ждем и не уменьшаем LifeTime
                await UniTask.Yield();
            }
        }
    }

    protected internal virtual void RaiseSetCreateEvent() { }
   
    protected virtual void PerformDamageCheck() { }
}
