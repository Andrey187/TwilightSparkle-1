using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PoolObject<T> where T : Component
{
    private static PoolObject<T> _instance;
    public static PoolObject<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PoolObject instance has not been created!");
            }
            return _instance;
        }
    }

    [Inject] private DiContainer _diContainer;
    private List<T> _objectPrefab;
    private Transform _parentObject;
    private string _containerName;
    private Queue<T> _objectPool;
    private int _currentPrefabIndex = 0;

    private static GameObject _poolContainerPrefab;
    private static GameObject _poolContainerInstance;

    public PoolObject(List<T> objectPrefab, Transform parentObject, string containerName,  DiContainer diContainer)
    {
        _objectPool = new Queue<T>();
        _objectPrefab = objectPrefab;
        _parentObject = parentObject;
        _containerName = containerName;
        _diContainer = diContainer;
        InitializeNextObjectDelayed();
    }

    public static void CreateInstance(T prefab, Transform parentObject, string containerName, DiContainer diContainer)
    {
        CreateInstance(new List<T> { prefab }, parentObject, containerName, diContainer);
    }

    public static void CreateInstance(List<T> prefabs, Transform parentObject, string containerName, DiContainer diContainer)
    {
        if (_instance == null)
        {
            CreatePoolContainer(parentObject, containerName);
            _instance = new PoolObject<T>(prefabs, parentObject, containerName, diContainer);
            ObjectPoolManager.Instance.RegisterPool(_instance);
        }
    }

    //Call in ObjectPoolManager
    public void ResetInstance()
    {
        if (_instance != null)
        {
            _instance.ClearInstances();
            _instance = null;
        }
    }

    public void ClearInstances()
    {
        foreach (var obj in _objectPool)
        {
            Object.Destroy(obj.gameObject);
        }

        _objectPool.Clear();

        if (_poolContainerInstance != null)
        {
            Object.Destroy(_poolContainerInstance.gameObject);
            _poolContainerInstance = null;
        }
    }

    public T GetObjects(Vector3 position, object obj, bool autoExpandPool)
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
                    if (!item.gameObject.activeSelf && item.GetType() == prefab.GetType())
                    {
                        result = item;
                        break;
                    }
                }

                // if no inactive object is found, create a new one
                if (result == null && autoExpandPool)
                {
                    result = _diContainer.InstantiatePrefab(prefab, _poolContainerInstance.transform).GetComponent<T>();
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
                    if (!item.gameObject.activeSelf && item.GetType() == prefab.GetType())
                    {
                        result = item;
                        break;
                    }
                }

                // if no inactive object is found, create a new one
                if (result == null && autoExpandPool)
                {
                    result = _diContainer.InstantiatePrefab(prefab, _poolContainerInstance.transform).GetComponent<T>();
                    result.transform.SetParent(_poolContainerInstance.transform, false);
                    _objectPool.Enqueue(result);
                }
            }
        }

        if (result != null)
        {
            result.transform.position = position;
            result.gameObject.SetActive(true);
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

    private async void InitializeNextObjectDelayed()
    {
        while (_currentPrefabIndex < _objectPrefab.Count)
        {
            T prefab = _objectPrefab[_currentPrefabIndex];
            T obj = _diContainer.InstantiatePrefab(prefab, _poolContainerInstance.transform).GetComponent<T>();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolContainerInstance.transform, false);
            _objectPool.Enqueue(obj);

            _currentPrefabIndex++;

            // Wait for the specified delay before initializing the next object
            await Task.Delay(20);
        }
    }
}

