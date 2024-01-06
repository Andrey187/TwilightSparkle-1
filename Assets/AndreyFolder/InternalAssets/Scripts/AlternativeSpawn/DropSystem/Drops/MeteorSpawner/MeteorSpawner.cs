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
    [SerializeField] private bool _autoExpand;
    private PoolObject<Meteor> _objectsPool;
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
            _meteors.Add(_meteorPrefab);
        }

        PoolObject<Meteor>.CreateInstance(_meteors, gameObject.transform, _meteors.First().name + "_Container", _diContainer);
        _objectsPool = PoolObject<Meteor>.Instance;
    }

    public IEnumerator SpawnMeteor()
    {
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < _meteorCount; i++)
        {
            Meteor activeMeteor = _objectsPool.GetObjects(_meteors[i].transform.position, _meteors[i], _autoExpand);
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
