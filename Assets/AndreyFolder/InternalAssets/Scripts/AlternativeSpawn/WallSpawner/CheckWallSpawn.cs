using UnityEngine;
using UnityEngine.AI;

public class CheckWallSpawn : MonoBehaviour
{
    public delegate void WallsSpawnedEventHandler(GameObject wallsObject);
    public event WallsSpawnedEventHandler WallsSpawnedEvent;

    [SerializeField] private NavMeshSurface _navMeshSurface;

    private void Start()
    {
        // Subscribe to the walls spawned event
        EventManager.Instance.WallsSpawnedEvent += HandleWallsSpawned;
    }

    private void HandleWallsSpawned(GameObject obj)
    {
        if (WallManager.WallsSpawnedCount >= WallManager.TotalWallsToSpawn)
        {
            EventManager.Instance.WallsSpawnedEvent -= HandleWallsSpawned;
            _navMeshSurface.BuildNavMesh();
            WallsSpawnedEvent?.Invoke(obj);
        }
    }
}
