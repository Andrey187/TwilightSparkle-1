using System;
using UnityEngine;

public class OuterWallSpawn : WallsSpawner
{
    [SerializeField] private CalculateSpawnPositionForOuterWall _calculatePos;
    private IObjectFactory _objectFactory;

    private void Start()
    {
        WallManager.TotalWallsToSpawn = _countOfObjects;
        _objectFactory = new ObjectsFactory(_prefabsWall);
        _calculatePos._listOuterWall = _objectFactory.CreateObjects(Vector3.zero, WallManager.TotalWallsToSpawn);
        InitCreatePool();
        _calculatePos.InitCollider();
        SpawnObjects();
        _calculatePos.ParamsForGround();
        _calculatePos.ResizeWall();
        _calculatePos.StartCoroutine(_calculatePos.PositionOnSide(_calculatePos._listOuterWall));
    }

    private void InitCreatePool()
    {
        foreach(Transform obj in _calculatePos._listOuterWall)
        {
            CreatePool(obj.GetComponent<Transform>(), WallManager.TotalWallsToSpawn,
            gameObject.transform, "WallsContainer");
        }
    }
    private void SpawnObjects()
    {
        Action<GameObject> setObjectActive = EventManager.Instance.WallsSpawned;
        for (int i =0; i < _calculatePos._listOuterWall.Length; i++)
        {
            _calculatePos._listOuterWall[i] = GetObjectFromPool(transform.position, _prefabsWall);
            setObjectActive?.Invoke(_calculatePos._listOuterWall[i].gameObject);
            _calculatePos.ParamsForWall(_calculatePos._listOuterWall[i]);
           WallManager.WallsSpawnedCount++;
        }
    }
}
