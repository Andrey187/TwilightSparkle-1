using UnityEngine;
using Zenject;

public abstract class BaseAbilityMovement : MonoCache, IAbilityMove
{
    [Inject] protected IAttackSystem _attackSystem;
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected Transform _startPosition;
    [SerializeField] protected Transform _endPosition;
    protected bool _injected = false;
    protected Rigidbody _thisRb;

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
        Vector3 direction = endPoint - startPoint;
        direction.Normalize();

        _thisRb.velocity = direction * _speed;
    }

    public virtual void MoveWithPhysics() { }
}
