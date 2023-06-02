using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsSpawner : MonoCache
{
    [SerializeField] private CalculateSpawnPositionForBots _spawnAreaCalculation;
    [SerializeField] private WaveSpawner _waveSpawner;
    private PoolObject<BaseEnemy> _botPool;
    private IObjectFactory _objectFactory;

    private Dictionary<WaveSpawner.Wave, List<BaseEnemy>> _spawnedBotsForWave;

    // Start is called before the first frame update
    void Start()
    {
        InitCreatePool();
    }

    private void InitCreatePool()
    {
        if (_waveSpawner.Waves == null)
        {
            Debug.LogWarning("No waves found to spawn bots for.");
            return;
        }

        // Initialize the dictionary with empty lists for each wave
        _spawnedBotsForWave = new Dictionary<WaveSpawner.Wave, List<BaseEnemy>>();
        foreach (WaveSpawner.Wave wave in _waveSpawner.Waves)
        {
            _spawnedBotsForWave[wave] = new List<BaseEnemy>();
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
            for (int j = 0; j < wave.SpawnLimit; j++)
            {
                BaseEnemy bot = _objectFactory.CreateObject(wave.Bot.position).GetComponent<BaseEnemy>();
                _spawnedBotsForWave[wave].Add(bot);
            }
        }
        BaseEnemy[] objects = _spawnedBotsForWave.SelectMany(pair => pair.Value).ToArray();
        PoolObject<BaseEnemy>.CreateInstance(objects, objects.Length, gameObject.transform, "Bots");
        _botPool = PoolObject<BaseEnemy>.Instance;
    }

    public IEnumerator SpawnObjects(WaveSpawner.Wave wave)
    {
        yield return new WaitForSeconds(0.2f);

        List<BaseEnemy> botsForWave = _spawnedBotsForWave[wave];
        BaseEnemy[] botPrefabs = botsForWave.ToArray();
        for (int j = 0; j < botPrefabs.Length; j++)
        {

            BaseEnemy botPrefab = botPrefabs[j].GetComponent<BaseEnemy>();

            // Check if there are any inactive bot objects in the pool that match the current wave and prefab
            BaseEnemy inactiveBot = _botPool.GetObjects(Vector3.zero, botPrefab);

            wave.SpawnMethod.NewUnitCircle();
            wave.SpawnMethod.SpawnEnemies(wave);
            wave.SpawnMethod.GroundCheck();

            if (wave.SpawnMethod.ColliderCheck(inactiveBot.gameObject))
            {
                Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
                setObjectActive?.Invoke(inactiveBot.gameObject, true);
            }
            else
            {
                inactiveBot.gameObject.SetActive(false);
                PoolObject<BaseEnemy>.Instance.ReturnObject(inactiveBot);
            }
        }
    }
}
