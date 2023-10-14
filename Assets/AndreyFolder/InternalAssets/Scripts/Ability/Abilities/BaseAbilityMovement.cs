using UnityEngine;
using Zenject;

public abstract class BaseAbilityMovement : MonoCache, IAbilityMove
{
    [Inject] protected IAttackSystem _attackSystem;
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected Transform _startPosition;
    [SerializeField] protected Transform _endPosition;
    [Inject] protected IGamePause _gamePause;
    protected bool _injected = false;
    protected Rigidbody _thisRb;
    protected Vector3 _direction;

    protected void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    private void InjectDependencies()
    {
        _startPosition = _attackSystem.StartAttackPoint;
        _endPosition = _attackSystem.EndAttackPoint;
    }
    protected override void OnEnabled()
    {
        EnsureDependenciesInjected();
        MoveWithPhysics(_endPosition.position, _startPosition.position);
    }

    protected override void FixedRun()
    {
        if (_gamePause.IsPaused)
        {
            _thisRb.velocity = Vector3.zero;
            return;
        }

        Vector3 newPosition = _thisRb.position + _direction * _speed * Time.fixedDeltaTime;
        _thisRb.MovePosition(newPosition);
    }

    protected void EnsureDependenciesInjected()
    {
        if (!_injected)
        {
            InjectDependencies();
            _injected = true;
        }
    }

    public virtual void MoveWithPhysics(Vector3 endPoint, Vector3 startPoint)
    {
        _direction = endPoint - startPoint;
        _direction.Normalize();
    }

}
