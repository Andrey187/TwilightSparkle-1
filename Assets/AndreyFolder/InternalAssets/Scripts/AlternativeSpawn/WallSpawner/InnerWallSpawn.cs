using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerWallSpawn : WallsSpawner
{
    [SerializeField] private CalculateSpawnPositionForInnerWall _calculatePos;
    [SerializeField] private LayerMask _objectLayerMaskCheckColliders;
    [SerializeField] private float _colliderHitRadius = 0.5f;

    private HashSet<Transform> _spawnedObjects = new HashSet<Transform>();
    private Transform[] _objectsToPool;
    private Vector3 _posPrefabs;
    private IObjectFactory _objectFactory;
    private IRotationStrategy _rotationStrategies;

    private void Start()
    {
        WallManager.TotalWallsToSpawn = _countOfObjects;

        _objectFactory = new ObjectsFactory(_prefabsWall);
        _objectsToPool = _objectFactory.CreateObjects(_posPrefabs, WallManager.TotalWallsToSpawn);

        InitCreatePool();
        Invoke("SpawnObjects", 0.2f);
    }

    private void InitCreatePool()
    {
        foreach (Transform obj in _objectsToPool)
        {
            CreatePool(obj, WallManager.TotalWallsToSpawn, gameObject.transform, "Object Pool");
        }
    }
    
    private void SpawnObjects()
    {
        Collider[] hitColliders = new Collider[WallManager.TotalWallsToSpawn];
        Collider[] sameColliders = new Collider[WallManager.TotalWallsToSpawn];
        Action<GameObject> setObjectActive = EventManager.Instance.WallsSpawned;

        for (int i = _spawnedObjects.Count; i < WallManager.TotalWallsToSpawn; i++)
        {
            _posPrefabs = _calculatePos.RandomSetPositions(_objectFactory, i);

            // Perform a SphereCast to check for collisions in the given radius
            int numColliders = Physics.OverlapSphereNonAlloc(_posPrefabs, _colliderHitRadius, hitColliders, _objectLayerMaskCheckColliders);

            // If there are no colliders in the given radius, create the new object
            if (numColliders == 0)
            {
                _rotationStrategies = _calculatePos.GetRandomRotation();

                // Create the new object at the random position
                Transform newObj = GetObjectFromPool(_posPrefabs, _objectsToPool);
                newObj.rotation = _rotationStrategies.GetRandomRotation();

                // Add to list
                _spawnedObjects.Add(newObj);

                WallManager.WallsSpawnedCount++;

                // Move overlapping objects away from each other
                foreach (Transform obj in _spawnedObjects)
                {
                    if (obj == newObj)
                        continue;

                    float distance = Vector3.Distance(obj.position, newObj.position);
                    if (distance < _colliderHitRadius)
                    {
                        Vector3 direction = (newObj.position - obj.position).normalized;
                        newObj.position += direction * (_colliderHitRadius - distance);
                    }
                }

                setObjectActive?.Invoke(newObj.gameObject);
            }
        }
    }
}

//Use a HashSet instead of a List for _spawnedObjects.
//This will provide faster lookup times when checking for duplicates during sphere casting.

