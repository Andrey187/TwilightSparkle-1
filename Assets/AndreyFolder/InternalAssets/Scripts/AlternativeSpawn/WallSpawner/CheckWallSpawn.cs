using UnityEngine;
using UnityEngine.AI;

public class CheckWallSpawn : MonoCache
{
    public delegate void WallsSpawnedEventHandler(GameObject wallsObject);
    public event WallsSpawnedEventHandler WallsSpawnedEvent;

    [SerializeField] private NavMeshSurface _navMeshSurface;
    private EventManager _eventManager;

    private void Start()
    {
        _eventManager = EventManager.Instance;
        // Subscribe to the walls spawned event
        _eventManager.WallsSpawnedEvent += HandleWallsSpawned;
    }

    protected override void OnDisabled()
    {
        _eventManager.WallsSpawnedEvent -= HandleWallsSpawned;
    }

    private void HandleWallsSpawned(GameObject obj)
    {
        if (WallManager.WallsSpawnedCount >= WallManager.TotalWallsToSpawn)
        {
            EventManager.Instance.WallsSpawnedEvent -= HandleWallsSpawned;
            if (_navMeshSurface != null)
            {
                _navMeshSurface.BuildNavMesh();
            }
            WallsSpawnedEvent?.Invoke(obj);
        }
    }
}
