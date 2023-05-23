using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    private Action<GameObject> _onObjectSetActive;//For check object active or enable
    private Action<GameObject> _wallsSpawnedEvent;//For Wall Spawn

    private Action<BaseEnemy,int, IAbility, IDoTEffect> _takeAbilityDamage;//For TakeDamage

    public List<GameObject> _subscribedObjects = new List<GameObject>();//For Wall Spawn

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("EventManager");
            _instance = obj.AddComponent<EventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventManager>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        WallManager.WallsSpawnedCount = 0;
        _subscribedObjects = new List<GameObject>();
    }

    public event Action<GameObject> OnObjectSetActive
    {
        add { _onObjectSetActive += value; }
        remove { _onObjectSetActive -= value; }
    }

    public event Action<GameObject> WallsSpawnedEvent
    {
        add { _wallsSpawnedEvent += value; }
        remove { _wallsSpawnedEvent -= value; }
    }

    public event Action<BaseEnemy,int, IAbility, IDoTEffect> TakeAbilityDamage
    {
        add { _takeAbilityDamage += value; }
        remove { _takeAbilityDamage -= value; }
    }

    public void SetObjectActive(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
        _onObjectSetActive?.Invoke(obj);
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

    public void AbillityDamage(BaseEnemy enemy,int amount , IAbility ability, IDoTEffect doTEffect)
    {
        _takeAbilityDamage?.Invoke(enemy,amount, ability, doTEffect);
    }
}
