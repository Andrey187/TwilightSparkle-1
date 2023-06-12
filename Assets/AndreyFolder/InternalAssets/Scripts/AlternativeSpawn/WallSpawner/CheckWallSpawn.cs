using UnityEngine;
using UnityEngine.AI;

public class CheckWallSpawn : MonoCache
{
    public delegate void WallsSpawnedEventHandler(GameObject wallsObject);
    public event WallsSpawnedEventHandler WallsSpawnedEvent;

    [SerializeField] private NavMeshSurface _navMeshSurface;
    private WallsEventManager _wallsEventManager;

    private void Start()
    {
        _wallsEventManager = WallsEventManager.Instance;
        // Subscribe to the walls spawned event
        _wallsEventManager.WallsSpawnedEvent += HandleWallsSpawned;
    }

    protected override void OnDisabled()
    {
        _wallsEventManager.WallsSpawnedEvent -= HandleWallsSpawned;
    }

    private void HandleWallsSpawned(GameObject obj)
    {
        if (WallManager.WallsSpawnedCount >= WallManager.TotalWallsToSpawn)
        {
            WallsEventManager.Instance.WallsSpawnedEvent -= HandleWallsSpawned;
            if (_navMeshSurface != null)
            {
                _navMeshSurface.BuildNavMesh();
            }
            WallsSpawnedEvent?.Invoke(obj);
        }
    }
}
