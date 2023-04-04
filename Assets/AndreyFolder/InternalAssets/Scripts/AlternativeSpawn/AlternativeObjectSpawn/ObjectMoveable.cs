using UnityEngine;
using UnityEngine.AI;

public class ObjectMoveable : MonoCache
{
    [SerializeField] private Position _targetPosition;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    private void Awake() => _navMeshAgent = Get<NavMeshAgent>();

    protected override void Run()
    {
        if (gameObject.activeSelf && _navMeshAgent.isOnNavMesh)
            _navMeshAgent.destination = _targetPosition.Value;
    }
}
