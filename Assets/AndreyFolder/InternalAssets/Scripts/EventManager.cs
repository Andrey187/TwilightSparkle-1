using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<GameObject> _subscribedObjects = new List<GameObject>();//For Wall Spawn
    private Action<GameObject> _onObjectSetActive;//For check object active or enable
    private Action<GameObject> _wallsSpawnedEvent;//For Wall Spawn

    private Action<BaseEnemy, int, IAbility, IDoTEffect> _takeAbilityDamage;//For TakeDamage
    private Action _playerDeath;
    private Action _abilityChoice;
    private Action<GameObject> _objectCreated;
    private Action<GameObject> _objectDestroyed;
    private Action<GameObject> _healthBarCreated;
    private Action<GameObject> _healthBarDestroyed;

    private static EventManager _instance;
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

    public event Action<BaseEnemy, int, IAbility, IDoTEffect> TakeAbilityDamage
    {
        add { _takeAbilityDamage += value; }
        remove { _takeAbilityDamage -= value; }
    }

    public event Action PlayerDeath
    {
        add { _playerDeath += value; }
        remove { _playerDeath -= value; }
    }

    public event Action AbilityChoice
    {
        add { _abilityChoice += value; }
        remove { _abilityChoice -= value; }
    }

    #region{Enemy}
    public event Action<GameObject> ObjectCreated
    {
        add { _objectCreated += value; }
        remove { _objectCreated -= value; }
    }

    public event Action<GameObject> ObjectDestroyed
    {
        add { _objectDestroyed += value; }
        remove { _objectDestroyed -= value; }
    }
    #endregion

    #region{UI}
    public event Action<GameObject> HealthBarCreated
    {
        add { _healthBarCreated += value; }
        remove { _healthBarCreated -= value; }
    }

    public event Action<GameObject> HealthBarDestroyed
    {
        add { _healthBarDestroyed += value; }
        remove { _healthBarDestroyed -= value; }
    }
    #endregion

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

    public void AbillityDamage(BaseEnemy enemy, int amount, IAbility ability, IDoTEffect doTEffect)
    {
        _takeAbilityDamage?.Invoke(enemy, amount, ability, doTEffect);
    }

    public void PlayerDie()
    {
        _playerDeath?.Invoke();
    }

    public void AbilityChoiceUI()
    {
        _abilityChoice?.Invoke();
    }
    #region{Enemy}
    public void CreatedObject(GameObject obj)
    {
        _objectCreated?.Invoke(obj);
    }

    public void DestroyedObject(GameObject obj)
    {
        _objectDestroyed?.Invoke(obj);
    }
    #endregion

    #region{UI}
    //UI HealthBarElements
    public void CreateHealthBar(GameObject obj)
    {
        _healthBarCreated?.Invoke(obj);
    }

    public void DestroyHealthBar(GameObject obj)
    {
        _healthBarDestroyed?.Invoke(obj);
    }
    #endregion
}
