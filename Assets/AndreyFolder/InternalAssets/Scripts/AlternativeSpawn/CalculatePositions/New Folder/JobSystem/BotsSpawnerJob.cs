using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class BotsSpawnerJob : MonoCache
{
    [SerializeField] private WaveSpawnerJob _waveSpawner;
    public float _spawnRadius = 1.5f;
    public float _groupSpawnCircleRadius = 20f;
    public const float distanceToCheckGround = 3f;
    public float _colliderHitRadius = 0.3f;
    public Transform _player;
    public NativeArray<float3> _randomInsideUnitCircle;
    public NativeArray<float3> _spawnCircleRadius;
    public NativeArray<float3> _botsSpawnInRandomPointOnCircle;
    public NativeArray<float3> _botsSpawnField;
    public NativeArray<float2> randomInsideUnitCircle2D;
    public NativeArray<float3> pos;
    public bool isGround = false;
    public RaycastHit hit;
    private SpawnJob spawnJob;
    private JobHandle jobHandle;

    private PoolObject<BaseEnemy> _botPool;
    private IObjectFactory _objectFactory;

    public Dictionary<WaveSpawnerJob.WaveJob, List<BaseEnemy>> SpawnedBotsForWave;

    [Inject] private DiContainer _diContainer;
    // Start is called before the first frame update
    private void Start()
    {
        SpawnedBotsForWave = new Dictionary<WaveSpawnerJob.WaveJob, List<BaseEnemy>>();
        InitCreatePool(); SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
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
        foreach (WaveSpawnerJob.WaveJob wave in _waveSpawner.Waves)
        {
            SpawnedBotsForWave[wave] = new List<BaseEnemy>();
        }

        // Create object pool for the wave
        for (int i = 0; i < _waveSpawner.Waves.Length; i++)
        {
            WaveSpawnerJob.WaveJob wave = _waveSpawner.Waves[i];
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
                SpawnedBotsForWave[wave].Add(bot);
            }
        }
        List<BaseEnemy> allBots = SpawnedBotsForWave.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseEnemy>.CreateInstance(allBots, 0, gameObject.transform, "Bots", _diContainer);
        _botPool = PoolObject<BaseEnemy>.Instance;
    }

    public IEnumerator SpawnObjects(WaveSpawnerJob.WaveJob wave)
    {
        yield return new WaitForSeconds(0.05f);
        List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];

        _randomInsideUnitCircle = new NativeArray<float3>(botsForWave.Count, Allocator.TempJob);
        _spawnCircleRadius = new NativeArray<float3>(botsForWave.Count, Allocator.TempJob);
        randomInsideUnitCircle2D = new NativeArray<float2>(botsForWave.Count, Allocator.TempJob);
        _botsSpawnInRandomPointOnCircle = new NativeArray<float3>(botsForWave.Count, Allocator.TempJob);
        pos = new NativeArray<float3>(botsForWave.Count, Allocator.TempJob);
        _botsSpawnField = new NativeArray<float3>(botsForWave.Count, Allocator.TempJob);

        for (int i = 0; i < botsForWave.Count; i++)
        {
            _randomInsideUnitCircle[i] = wave.SpawnMethod.NewUnitCircleJob();
            randomInsideUnitCircle2D[i] = UnityEngine.Random.insideUnitCircle;
        }

        spawnJob = new SpawnJob
        {
            spawnRadius = _spawnRadius,
            groupSpawnCircleRadius = _groupSpawnCircleRadius,
            distanceToCheckGround = distanceToCheckGround,
            colliderHitRadius = _colliderHitRadius,
            playerPos = _player.transform.position,
            randomInsideUnitCircle = _randomInsideUnitCircle,
            spawnCircleRadius = _spawnCircleRadius,
            botsSpawnInRandomPointOnCircle = _botsSpawnInRandomPointOnCircle,
            randomInsideUnitCircle2D = randomInsideUnitCircle2D,
            botsSpawnField = _botsSpawnField,
            Pos = pos,
            isGround = isGround
        };
        int batchCount = Mathf.Max(1, Environment.ProcessorCount);
        JobHandle jobHandle = spawnJob.Schedule(botsForWave.Count, batchCount);

        jobHandle.Complete();
        for (int j = 0; j < wave.SpawnLimit; j++)
        {
            // Check if there are any inactive bot objects in the pool that match the current wave and prefab
            //wave.SpawnMethod.NewUnitCircle();
            //wave.SpawnMethod.SpawnEnemies();

            wave.SpawnMethod.GroundCheck(_botsSpawnInRandomPointOnCircle[j]);

            BaseEnemy inactiveBot = GetInactiveBot(wave);

            Action<GameObject> objectCreated = EnemyEventManager.Instance.CreatedObject;
            objectCreated?.Invoke(inactiveBot.gameObject);

            if (wave.SpawnMethod.ColliderCheck(inactiveBot, _botsSpawnInRandomPointOnCircle[j]))
            {
                Action<GameObject, bool> setObjectActive = EnemyEventManager.Instance.SetObjectActive;
                setObjectActive?.Invoke(inactiveBot.gameObject, true);
            }
            else
            {
                _botPool.ReturnObject(inactiveBot);
            }
        }
        jobHandle.Complete(); // Wait for the job to finish before proceeding


        _randomInsideUnitCircle.Dispose();
        _spawnCircleRadius.Dispose();
        randomInsideUnitCircle2D.Dispose();
        _botsSpawnInRandomPointOnCircle.Dispose();
        pos.Dispose();
        _botsSpawnField.Dispose();
    }

    private BaseEnemy GetInactiveBot(WaveSpawnerJob.WaveJob wave)
    {
        List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];
        for (int i = 0; i < botsForWave.Count; i++)
        {
            BaseEnemy bot = botsForWave[i];

            if (bot.gameObject.activeSelf)
            {
                BaseEnemy inactiveBot = _botPool.GetObjects(Vector3.zero, bot);
                inactiveBot.GetComponentInChildren<MeshRenderer>().sharedMaterial = wave._objMaterial;
                return inactiveBot;
            }
        }
        return null;
    }
}
