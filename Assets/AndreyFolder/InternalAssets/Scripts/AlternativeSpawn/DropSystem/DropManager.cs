using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropManager : MonoCache
{
    [SerializeField] private List<BaseDrop> DropPrefab;
    [SerializeField] private int _countForFirstAid;
    [SerializeField] private int _countForBomb;

    [SerializeField] private int enemiesKilledCount = 0;

    private PoolObject<BaseDrop> _objectsToPool;
    private IObjectFactory _objectFactory;
    private DropEventManager _eventManager;

    private void Start()
    {
        enemiesKilledCount = 0;
        foreach (var drop in DropPrefab)
        {
            _objectFactory = new ObjectsFactory(drop.GetComponent<BaseDrop>().transform);
            BaseDrop baseDrop = _objectFactory.CreateObject(drop.transform.position).GetComponent<BaseDrop>();

            PoolObject<BaseDrop>.CreateInstance(baseDrop, 0, gameObject.transform, baseDrop.name + "_Drops");
            _objectsToPool = PoolObject<BaseDrop>.Instance;
        }
        _eventManager = DropEventManager.Instance;
        _eventManager.DropCreated += EnemyKilled;
    }

    protected override void OnDisabled()
    {
        _eventManager.DropCreated -= EnemyKilled;
    }

    private void EnemyKilled(GameObject obj)
    {
        enemiesKilledCount++;

        if (enemiesKilledCount % _countForFirstAid == 0)
        {
            DropFirstAidKit(obj.transform.position);
        }

        if (enemiesKilledCount % _countForBomb == 0)
        {
            DropExplosiveDevice(obj.transform.position);
        }
    }

    private void DropFirstAidKit(Vector3 pos)
    {
        FirstAidDrop firstAidDrop = DropPrefab.FirstOrDefault(drop => drop is FirstAidDrop) as FirstAidDrop;
        if (firstAidDrop != null)
        {
            // Instantiate and activate the first aid kit prefab
            BaseDrop firstAidKit = _objectsToPool.GetObjects(pos, firstAidDrop);
        }
    }

    private void DropExplosiveDevice(Vector3 pos)
    {
        MeteorDrop bombDrop = DropPrefab.FirstOrDefault(drop => drop is MeteorDrop) as MeteorDrop;
        if (bombDrop != null)
        {
            // Instantiate and activate the first aid kit prefab
            BaseDrop bombDropKit = _objectsToPool.GetObjects(pos, bombDrop);
        }
    }
}
