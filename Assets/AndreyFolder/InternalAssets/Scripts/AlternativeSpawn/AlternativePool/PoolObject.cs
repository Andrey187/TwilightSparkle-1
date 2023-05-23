using System.Collections.Generic;
using UnityEngine;

public class PoolObject<T> where T : Component
{
    public static PoolObject<T> Instance { get; private set; }

    private T[] _objectPrefab;
    private int _poolLimit;
    private Transform _parentObject;
    private string _containerName;
    private Queue<T> _objectPool;
    private Dictionary<T, int> _objectWaves = new Dictionary<T, int>();

    private static GameObject _poolContainerPrefab;
    private static GameObject _poolContainerInstance;

    public PoolObject(T[] objectPrefab, int poolLimit, Transform parentObject, string containerName)
    {
        _objectPrefab = objectPrefab;
        _poolLimit = poolLimit;
        _parentObject = parentObject;
        _containerName = containerName;
        InitializeObjectPool();
    }

    public static void CreateInstance(T prefab, int poolLimit, Transform parentObject, string containerName)
    {
        CreateInstance(new T[] { prefab }, poolLimit, parentObject, containerName);
    }

    public static void CreateInstance(T[] prefabs, int poolLimit, Transform parentObject, string containerName)
    {
        if (Instance == null)
        {
            CreatePoolContainer(parentObject, containerName);
            Instance = new PoolObject<T>(prefabs, poolLimit, parentObject, containerName);
        }
    }

    public T GetObjects(Vector3 position, object obj)
    {
        return GetObjects(position, obj, 0);
    }

    public T GetObjects(Vector3 position, object obj, int wave)
    {
        T result = null;

        // check if obj is an array
        if (obj != null)
        {
            if (obj.GetType().IsArray)
            {
                // cast obj to array and get a random prefab from it
                T[] prefabs = (T[])obj;

                // check if prefabs array is empty
                if (prefabs.Length == 0)
                {
                    Debug.LogError("Prefabs array is empty!");
                    return null;
                }

                T prefab = prefabs[Random.Range(0, prefabs.Length)];

                // try to get an inactive object from the pool
                foreach (T item in _objectPool)
                {
                    if (!item.gameObject.activeSelf && item.gameObject.CompareTag(prefab.gameObject.tag) && (!_objectWaves.ContainsKey(item) || _objectWaves[item] == wave))
                    {
                        result = item;
                        break;
                    }
                }

                // if no inactive object is found, create a new one
                if (result == null)
                {
                    result = Object.Instantiate(prefab, _poolContainerInstance.transform);
                    result.transform.SetParent(_poolContainerInstance.transform, false);
                    _objectPool.Enqueue(result);
                }
            }
            else
            {
                T prefab = (T)obj;

                // try to get an inactive object from the pool
                foreach (T item in _objectPool)
                {
                    //if (!item.gameObject.activeSelf && item.gameObject.CompareTag(prefab.gameObject.tag) && (!_objectWaves.ContainsKey(item) || _objectWaves[item] == wave))
                    if (!item.gameObject.activeSelf && item.GetType() == prefab.GetType() && (!_objectWaves.ContainsKey(item) || _objectWaves[item] == wave))
                    {
                        result = item;
                        break;
                    }
                }

                // if no inactive object is found, create a new one
                if (result == null)
                {
                    result = Object.Instantiate(prefab, _poolContainerInstance.transform);
                    result.transform.SetParent(_poolContainerInstance.transform, false);
                    _objectPool.Enqueue(result);
                }
            }
        }

        if (result != null)
        {
            result.transform.position = position;
            result.gameObject.SetActive(true);

            // add the object and the wave to the dictionary
            _objectWaves[result] = wave;
        }

        return result;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    private static void CreatePoolContainer(Transform parentObject, string containerName)
    {
        if (_poolContainerInstance == null)
        {
            _poolContainerPrefab = new GameObject(containerName);
            _poolContainerPrefab.transform.SetParent(parentObject.transform, false);
            _poolContainerInstance = _poolContainerPrefab;
        }
    }

    private void InitializeObjectPool()
    {
        _objectPool = new Queue<T>();

        for (int i = 0; i < _objectPrefab.Length; i++)
        {
            T prefab = _objectPrefab[i];
            T obj = Object.Instantiate(prefab, _poolContainerInstance.transform);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolContainerInstance.transform, false);
            _objectPool.Enqueue(obj);
        }
    }
}

//Using CompareTag is generally not considered resource-intensive because it only compares the tag of the game object,
//which is a string, and the string comparison is fast. In addition, 
//using CompareTag can be more efficient than using gameObject.name because Unity internally caches tags for faster comparisons,
//whereas the name of the game object can be changed at runtime.
//That being said, it's always a good practice to measure the performance of your code and optimize it if needed. 
//If you're experiencing performance issues with using CompareTag, 
//you can consider using other approaches such as storing the game object reference or ID instead of the tag.
