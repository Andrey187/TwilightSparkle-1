using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsSpawner : MonoCache
{
    [SerializeField] private CalculateSpawnPositionForBots _spawnAreaCalculation;
    [SerializeField] private WaveSpawner _waveSpawner;
    private PoolObject<BotWaveReference> _botPool;
    private IObjectFactory _objectFactory;

    private Dictionary<WaveSpawner.Wave, List<BotWaveReference>> _spawnedBotsForWave;

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
        _spawnedBotsForWave = new Dictionary<WaveSpawner.Wave, List<BotWaveReference>>();
        foreach (WaveSpawner.Wave wave in _waveSpawner.Waves)
        {
            _spawnedBotsForWave[wave] = new List<BotWaveReference>();
        }

        // Create object pool for the wave
        for (int i = 0; i < _waveSpawner.Waves.Length; i++)
        {
            WaveSpawner.Wave wave = _waveSpawner.Waves[i];
            _objectFactory = new ObjectsFactory(wave.Bot.GetComponent<BotWaveReference>().transform);
            if (wave.Bot == null)
            {
                Debug.LogWarning("No bots found for wave " + wave.Name);
                continue;
            }

            // Add the bots to the List
            for (int j = 0; j < wave.SpawnLimit; j++)
            {
                BotWaveReference bot = _objectFactory.CreateObject(wave.Bot.position).GetComponent<BotWaveReference>();
                // Set the wave index on the BotWaveReference component
                if (bot != null)
                {
                    bot.WaveIndex = i;
                }
                bot.Wave = wave;
                _spawnedBotsForWave[wave].Add(bot);
            }
        }
        BotWaveReference[] objects = _spawnedBotsForWave.SelectMany(pair => pair.Value).ToArray();
        PoolObject<BotWaveReference>.CreateInstance(objects, objects.Length, gameObject.transform, "Bots");
        _botPool = PoolObject<BotWaveReference>.Instance;
    }

    public IEnumerator SpawnObjects(WaveSpawner.Wave wave)
    {
        yield return new WaitForSeconds(0.1f);
        _spawnAreaCalculation.NewUnitCircle();
        _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField();
        _spawnAreaCalculation.GroundCheck();

        yield return new WaitForSeconds(0.2f);

        List<BotWaveReference> botsForWave = _spawnedBotsForWave[wave];

        if (botsForWave != null && _spawnedBotsForWave.ContainsKey(wave))
        {
            BotWaveReference[] botPrefabs = botsForWave.ToArray();
            for (int j = 0; j < botPrefabs.Length; j++)
            {
                BotWaveReference botPrefab = botPrefabs[j];

                // Check if there are any inactive bot objects in the pool that match the current wave and prefab
                BotWaveReference inactiveBot = _botPool.GetObjects(Vector3.zero, botPrefab, botPrefab.WaveIndex);
                if (_spawnAreaCalculation.ColliderCheck(inactiveBot.gameObject))
                {
                    Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
                    setObjectActive?.Invoke(inactiveBot.gameObject, true);
                }
                else
                {
                    inactiveBot.gameObject.SetActive(false);
                    PoolObject<BotWaveReference>.Instance.ReturnObject(inactiveBot);
                }
            }
        }
        else
        {
            Debug.LogWarning("No bots found for wave " + wave.Name);
        }
    }
}
