using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropManager : MonoCache
{
    [SerializeField] private List<BaseDrop> _dropPrefab;
    [SerializeField] private int _countForFirstAid;
    [SerializeField] private int _countForBomb;

    [SerializeField] private int enemiesKilledCount = 0;

    private PoolObject<BaseDrop> _objectsToPool;
    private IObjectFactory _objectFactory;
    private DropEventManager _eventManager;
    private Dictionary<Type, List<BaseDrop>> _cachePrefab = new Dictionary<Type, List<BaseDrop>>();

    private void Start()
    {
        _cachePrefab.Clear();
        enemiesKilledCount = 0;

        Invoke("InitPool", 7f);

        _eventManager = DropEventManager.Instance;
        _eventManager.DropCreated += EnemyKilled;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        _eventManager.DropCreated -= EnemyKilled;
    }

    private void InitPool()
    {
        for (int i = 0; i < _dropPrefab.Count; i++)
        {
            _objectFactory = new ObjectsFactory(_dropPrefab[i].GetComponent<BaseDrop>().transform);

            BaseDrop baseDrop = _objectFactory.CreateObject(_dropPrefab[i].transform.position).GetComponent<BaseDrop>();

            Type dropType = _dropPrefab[i].GetType();

            // Check if the key already exists in the dictionary
            if (!_cachePrefab.ContainsKey(dropType))
            {
                _cachePrefab[dropType] = new List<BaseDrop>();
            }

            // Add the baseDrop object to the list associated with the dropType key
            for (int j = 0; j < 10; j++)
            {
                _cachePrefab[dropType].Add(baseDrop);
            }
        }
        List<BaseDrop> allObjects = _cachePrefab.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseDrop>.CreateInstance(allObjects, 10, gameObject.transform, "_Drops");
        _objectsToPool = PoolObject<BaseDrop>.Instance;
    }

    private void EnemyKilled(GameObject obj)
    {
        enemiesKilledCount++;

        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);

        if (enemiesKilledCount % _countForFirstAid == 0)
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
            BaseDrop firstAidKit = _objectsToPool.GetObjects(pos, firstAidDrop);
        }
    }

    private void DropExplosiveDevice(Vector3 pos)
    {
        MeteorDrop bombDrop = _dropPrefab.FirstOrDefault(drop => drop is MeteorDrop) as MeteorDrop;
        if (bombDrop != null)
        {
            // Instantiate and activate the first aid kit prefab
            BaseDrop bombDropKit = _objectsToPool.GetObjects(pos, bombDrop);
        }
    }
}
