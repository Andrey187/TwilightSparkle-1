using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DropManager : MonoCache
{
    [SerializeField] public List<BaseDrop> _dropPrefab;
    [SerializeField] public int _countForFirstAid;
    [SerializeField] public int _countForBomb;
    [SerializeField] private bool _autoExpand;

    [SerializeField] private int enemiesKilledCount = 0;

    private int _originalCountForFirstAid;
    private int _originalCountForBomb;
    private PoolObject<BaseDrop> _objectsToPool;
    private DropEventManager _eventManager;
    private Dictionary<Type, List<BaseDrop>> _cachePrefab = new Dictionary<Type, List<BaseDrop>>();
    [Inject] private DiContainer _diContainer;

    public int ÑountForFirstAid
    {
        get => _countForFirstAid;
        set
        {
            // Ensure the new value is not below half of the original value
            _countForFirstAid = Mathf.Max(value, _originalCountForFirstAid / 2);
        }
    }

    public int CountForBomb
    {
        get => _countForBomb;
        set
        {
            // Ensure the new value is not below half of the original value
            _countForBomb = Mathf.Max(value, _originalCountForBomb / 2);
        }
    }

    private void Start()
    {
        enemiesKilledCount = 0;
        // Store the original values at the beginning
        _originalCountForFirstAid = _countForFirstAid;
        _originalCountForBomb = _countForBomb;

        Invoke("InitPool", 7f);

        _eventManager = DropEventManager.Instance;
        _eventManager.DropCreated += EnemyKilled;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        _cachePrefab.Clear();
        _eventManager.DropCreated -= EnemyKilled;
    }

    private void InitPool()
    {
        for (int i = 0; i < _dropPrefab.Count; i++)
        {
            Type dropType = _dropPrefab[i].GetType();

            // Check if the key already exists in the dictionary
            if (!_cachePrefab.ContainsKey(dropType))
            {
                _cachePrefab[dropType] = new List<BaseDrop>();
            }

            // Add the baseDrop object to the list associated with the dropType key
            for (int j = 0; j < 10; j++)
            {
                _cachePrefab[dropType].Add(_dropPrefab[i]);
            }
        }
        List<BaseDrop> allObjects = _cachePrefab.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseDrop>.CreateInstance(allObjects, gameObject.transform, "_Drops", _diContainer);
        _objectsToPool = PoolObject<BaseDrop>.Instance;
    }

    private void EnemyKilled(GameObject obj)
    {
        enemiesKilledCount++;

        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);

        if (enemiesKilledCount % ÑountForFirstAid == 0)
        {
            DropFirstAidKit(position);
        }

        if (enemiesKilledCount % _countForBomb == 0)
        {
            DropExplosiveDevice(position);
        }
    }

    private void DropFirstAidKit(Vector3 pos)
    {
        FirstAidDrop firstAidDrop = _dropPrefab.FirstOrDefault(drop => drop is FirstAidDrop) as FirstAidDrop;
        if (firstAidDrop != null)
        {
            // Instantiate and activate the first aid kit prefab
            BaseDrop firstAidKit = _objectsToPool.GetObjects(pos, firstAidDrop, _autoExpand);
        }
    }

    private void DropExplosiveDevice(Vector3 pos)
    {
        MeteorDrop bombDrop = _dropPrefab.FirstOrDefault(drop => drop is MeteorDrop) as MeteorDrop;
        if (bombDrop != null)
        {
            // Instantiate and activate the first aid kit prefab
            BaseDrop bombDropKit = _objectsToPool.GetObjects(pos, bombDrop, _autoExpand);
        }
    }
}
