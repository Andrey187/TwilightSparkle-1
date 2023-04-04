using UnityEngine;

public class ObjectsFactory : IObjectFactory
{
    private Transform[] _prefabs;
    private Transform _prefab;

    public ObjectsFactory(Transform[] prefabs)
    {
        _prefabs = prefabs;
    }
    public ObjectsFactory(Transform prefab)
    {
        _prefab = prefab;
    }

    public Transform[] CreateObjects(Vector3 pos, int count)
    {
        Transform[] spawnedObjects = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            spawnedObjects[i] = _prefabs[Random.Range(0, _prefabs.Length)];
        }
        return spawnedObjects;
    }
    public Transform CreateObject(Vector3 pos)
    {
        Transform spawnedObjects = _prefab;
        return spawnedObjects;
    }
}

