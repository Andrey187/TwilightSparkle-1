using UnityEngine;
using UnityEngine.AI;

public class BaseBotsMoveable : MonoCache
{
    [SerializeField] protected internal Position _targetPosition;
    [SerializeField] protected internal NavMeshAgent _navMeshAgent;
    [SerializeField] protected internal BaseEnemy _baseEnemy;

    protected virtual void Awake()
    {
        _navMeshAgent = Get<NavMeshAgent>();
        _baseEnemy = Get<BaseEnemy>();
    }

    protected override void Run()
    {
        if (gameObject.activeSelf && _navMeshAgent.isOnNavMesh)
            _navMeshAgent.destination = _targetPosition.Value;
            _baseEnemy.ChangeState(new WalkingState());
    }
}
