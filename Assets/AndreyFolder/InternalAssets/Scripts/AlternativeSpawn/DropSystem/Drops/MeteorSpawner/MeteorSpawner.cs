using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnMethod spawnMethod;
    [SerializeField] private Meteor _meteorPrefab;
    [SerializeField] private int _meteorCount;
    private PoolObject<Meteor> _objectsPool;
    private IObjectFactory _objectFactory;
    private HashSet<Meteor> _meteorsHashSet = new HashSet<Meteor>();

    private void Start()
    {
        _objectFactory = new ObjectsFactory(_meteorPrefab.transform);
        Meteor meteor = _objectFactory.CreateObject(_meteorPrefab.transform.position).GetComponent<Meteor>();

        PoolObject<Meteor>.CreateInstance(meteor, _meteorCount, gameObject.transform, meteor.name + "_Container");
        _objectsPool = PoolObject<Meteor>.Instance;

        for(int i = 0; i < _meteorCount; i++)
        {
            Meteor objectMeteor = _objectsPool.GetObjects(meteor.transform.position, meteor);
            if (objectMeteor != null)
            {
                _meteorsHashSet.Add(objectMeteor);
            }
        }
        foreach(Meteor meteorClone in _meteorsHashSet)
        {
            meteorClone.gameObject.SetActive(false);
        }
    }

    public IEnumerator SpawnMeteor()
    {
        yield return new WaitForSeconds(0.05f);
        
        foreach (Meteor meteor in _meteorsHashSet)
        {
            spawnMethod.NewUnitCircle();
            spawnMethod.SpawnEnemies();
            spawnMethod.GroundCheck();
            if (spawnMethod.ColliderCheck(meteor))
            {
                meteor.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
