using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsSpawner : MonoBehaviour
{
    [SerializeField] public Transform[] _prefabsWall; // This is the array of prefabs of the objects you want to create
    [SerializeField] public int _countOfObjects; // This is the number of objects you want to create

    private PoolObject<Transform> poolObject;

    public virtual void CreatePool<T>(T prefab,int poolLimit, Transform parentObject, string containerName) where T : Component
    {
        PoolObject<T>.CreateInstance(prefab, poolLimit, parentObject, containerName);
        poolObject = PoolObject<Transform>.Instance;
    }

    public virtual Transform GetObjectFromPool(Vector3 pos, object obj)
    {
        Transform newObj = poolObject.GetObjects(pos, obj);
        return newObj;
    }


    public virtual void DestroyObjects(HashSet<Transform> prefab)
    {
        foreach (Transform obj in prefab)
        {
            poolObject.ReturnObject(obj);
        }
        prefab.Clear();
    }
}
