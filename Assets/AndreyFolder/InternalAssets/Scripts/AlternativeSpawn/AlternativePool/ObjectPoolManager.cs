using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private List<object> activePools = new List<object>();

    private static ObjectPoolManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolManager");
                _instance = obj.AddComponent<ObjectPoolManager>();
            }
            return _instance;
        }
    }

    public void RegisterPool<T>(PoolObject<T> pool) where T : Component
    {
        activePools.Add(pool);
    }

    public void ClearAllPools()
    {
        foreach (var pool in activePools)
        {
            if (pool.GetType().IsGenericType && pool.GetType().GetGenericTypeDefinition() == typeof(PoolObject<>))
            {
                var resetMethod = pool.GetType().GetMethod("ResetInstance");
                if (resetMethod != null)
                {
                    resetMethod.Invoke(pool, null);
                }
            }
        }
        activePools.Clear();
    }
}
