using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private SpawnMethod spawnMethod;
    [SerializeField] private Meteor _meteorPrefab;
    [SerializeField] private int _meteorCount;
    private PoolObject<Meteor> _objectsPool;
    private IObjectFactory _objectFactory;
    private List<Meteor> _meteors = new List<Meteor>();

    [Inject] private DiContainer _diContainer;
    private void Start()
    {
        Invoke("InitPool", 15f);
    }

    private void OnDestroy()
    {
        _meteors.Clear();
    }

    private void InitPool()
    {
        for (int i = 0; i < _meteorCount; i++)
        {
            _objectFactory = new ObjectsFactory(_meteorPrefab.transform);
            Meteor meteor = _objectFactory.CreateObject(_meteorPrefab.transform.position).GetComponent<Meteor>();
            _meteors.Add(meteor);
        }

        PoolObject<Meteor>.CreateInstance(_meteors, _meteors.Count, gameObject.transform, _meteors.First().name + "_Container", _diContainer);
        _objectsPool = PoolObject<Meteor>.Instance;
    }

    public IEnumerator SpawnMeteor()
    {
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < _meteorCount; i++)
        {
            Meteor activeMeteor = _objectsPool.GetObjects(_meteors[i].transform.position, _meteors[i]);
            spawnMethod.NewUnitCircle();
            spawnMethod.SpawnPrefabs();
            spawnMethod.GroundCheck();

            if (spawnMethod.ColliderCheck(activeMeteor))
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
