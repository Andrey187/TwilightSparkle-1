using UnityEngine;
using UnityEngine.AI;

public class BotsMoveable : MonoCache
{
    [SerializeField] private Position _targetPosition;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    private void Awake()
    {
        _navMeshAgent = ChildrenGet<NavMeshAgent>();
        _targetPosition = Find<PositionWritter>()._position;
    }

    protected override void Run()
    {
        if (gameObject.activeSelf && _navMeshAgent.isOnNavMesh)
            _navMeshAgent.destination = _targetPosition.Value;
    }
}
