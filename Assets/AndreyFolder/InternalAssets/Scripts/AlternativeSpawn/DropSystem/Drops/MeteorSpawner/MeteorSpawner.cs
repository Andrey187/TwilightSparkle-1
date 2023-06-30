using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnMethod spawnMethod;
    [SerializeField] private Meteor _meteorPrefab;
    [SerializeField] private int _meteorCount;
    private PoolObject<Meteor> _objectsPool;
    private IObjectFactory _objectFactory;
    private List<Meteor> _meteors = new List<Meteor>();

    private void Start()
    {
        _meteors.Clear();
        for (int i = 0; i < _meteorCount; i++)
        {
            _objectFactory = new ObjectsFactory(_meteorPrefab.transform);
            Meteor meteor = _objectFactory.CreateObject(_meteorPrefab.transform.position).GetComponent<Meteor>();
            _meteors.Add(meteor);
        }
        Meteor[] meteors = _meteors.ToArray();
        PoolObject<Meteor>.CreateInstance(meteors, meteors.Length, gameObject.transform, meteors.First().name + "_Container");
        _objectsPool = PoolObject<Meteor>.Instance;
    }

    public IEnumerator SpawnMeteor()
    {
        yield return new WaitForSeconds(0.05f);

        Meteor[] meteors = _meteors.ToArray();
        for (int i = 0; i < _meteorCount; i++)
        {
            Meteor activeMeteor = _objectsPool.GetObjects(meteors[i].transform.position, meteors[i]);
            spawnMethod.NewUnitCircle();
            spawnMethod.SpawnEnemies();
            spawnMethod.GroundCheck();

            if (spawnMethod.ColliderCheck(activeMeteor))
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
