using UnityEngine;
using Zenject;

public abstract class BaseAbilityMovement : MonoCache, IMovable
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
        MoveWithPhysics(_endPosition, _startPosition);
    }

    protected void EnsureDependenciesInjected()
    {
        if (!_injected)
        {
            InjectDependencies();
            _injected = true;
        }
    }

    public virtual void MoveWithPhysics(Transform endPoint, Transform startPoint)
    {
        Vector3 direction = endPoint.position - startPoint.position;
        direction.Normalize();

        _thisRb.velocity = direction * _speed;
    }

    public virtual void MoveWithPhysics() { }
}
