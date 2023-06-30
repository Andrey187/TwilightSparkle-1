using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsSpawner : MonoCache
{
    [SerializeField] private WaveSpawner _waveSpawner;
    private PoolObject<BaseEnemy> _botPool;
    private IObjectFactory _objectFactory;

    public Dictionary<WaveSpawner.Wave, List<BaseEnemy>> SpawnedBotsForWave;
    // Start is called before the first frame update
    private void Start()
    {
        SpawnedBotsForWave = new Dictionary<WaveSpawner.Wave, List<BaseEnemy>>();
        InitCreatePool();
    }

    protected override void OnDisabled()
    {
        SpawnedBotsForWave.Clear();
    }

    private void InitCreatePool()
    {
        if (_waveSpawner.Waves == null)
        {
            Debug.LogWarning("No waves found to spawn bots for.");
            return;
        }
       
        // Initialize the dictionary with empty lists for each wave
        foreach (WaveSpawner.Wave wave in _waveSpawner.Waves)
        {
            SpawnedBotsForWave[wave] = new List<BaseEnemy>();
        }

        // Create object pool for the wave
        for (int i = 0; i < _waveSpawner.Waves.Length; i++)
        {
            WaveSpawner.Wave wave = _waveSpawner.Waves[i];
            _objectFactory = new ObjectsFactory(wave.Bot.GetComponent<BaseEnemy>().transform);
            if (wave.Bot == null)
            {
                Debug.LogWarning("No bots found for wave " + wave.Name);
                continue;
            }

            // Add the bots to the List
            for (int j = 0; j < 350; j++)
            {
                BaseEnemy bot = _objectFactory.CreateObject(wave.Bot.position).GetComponent<BaseEnemy>();
                SpawnedBotsForWave[wave].Add(bot);
            }
        }
        BaseEnemy[] objects = SpawnedBotsForWave.SelectMany(pair => pair.Value).ToArray();
        PoolObject<BaseEnemy>.CreateInstance(objects, 0, gameObject.transform, "Bots");
        _botPool = PoolObject<BaseEnemy>.Instance;
    }

    public IEnumerator SpawnObjects(WaveSpawner.Wave wave)
    {
        yield return new WaitForSeconds(0.05f);
        List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];
        BaseEnemy[] botPrefabs = botsForWave.ToArray();

        for (int j = 0; j < wave.SpawnLimit; j++)
        {
            BaseEnemy botPrefab = botPrefabs[j];
            // Check if there are any inactive bot objects in the pool that match the current wave and prefab
            wave.SpawnMethod.NewUnitCircle();
            wave.SpawnMethod.SpawnEnemies();
            wave.SpawnMethod.GroundCheck();
            BaseEnemy inactiveBot = _botPool.GetObjects(Vector3.zero, botPrefab);
            inactiveBot.GetComponentInChildren<MeshRenderer>().sharedMaterial = wave._objMaterial;


            Action<GameObject> objectCreated = EnemyEventManager.Instance.CreatedObject;
            objectCreated?.Invoke(inactiveBot.gameObject);

            if (wave.SpawnMethod.ColliderCheck(inactiveBot))
            {
                Action<GameObject, bool> setObjectActive = EnemyEventManager.Instance.SetObjectActive;
                setObjectActive?.Invoke(inactiveBot.gameObject, true);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                _botPool.ReturnObject(inactiveBot);
            }
        }
    }
}
