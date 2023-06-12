using System;
using System.Collections.Generic;
using UnityEngine;

public class WallsEventManager : MonoBehaviour
{
    public List<GameObject> _subscribedObjects = new List<GameObject>();
    private Action<GameObject> _wallsSpawnedEvent;

    private static WallsEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("WallsEventManager");
            _instance = obj.AddComponent<WallsEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static WallsEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WallsEventManager>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        WallManager.WallsSpawnedCount = 0;
        _subscribedObjects = new List<GameObject>();
    }

    public event Action<GameObject> WallsSpawnedEvent
    {
        add { _wallsSpawnedEvent += value; }
        remove { _wallsSpawnedEvent -= value; }
    }

    public void WallsSpawned(GameObject obj)
    {
        _subscribedObjects.Add(obj);
        _wallsSpawnedEvent?.Invoke(obj);

        if (WallManager.WallsSpawnedCount >= WallManager.TotalWallsToSpawn)
        {
            WallManager.WallsSpawnedCount = 0;
            _wallsSpawnedEvent?.Invoke(null);
        }
    }
}
