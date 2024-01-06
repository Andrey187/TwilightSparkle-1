using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BotsSpawner : MonoCache
{
    [SerializeField] private bool _autoExpand;
    private PoolObject<BaseEnemy> _botPool;
    private IObjectFactory _objectFactory;

    public Dictionary<WaveSpawner.Wave, List<BaseEnemy>> SpawnedBotsForWave;
    private Action<GameObject> _objectCreated;
    private Action<IEnemy, bool> _setObjectActive;

    [Inject] private IWaveSpawner _waveSpawner;
    [Inject] private DiContainer _diContainer;

    private void Start()
    {
        SpawnedBotsForWave = new Dictionary<WaveSpawner.Wave, List<BaseEnemy>>();
        Invoke("InitCreatePool", 1.5f);
        _objectCreated = EnemyEventManager.Instance.CreatedObject;
        _setObjectActive = EnemyEventManager.Instance.SetObjectActive;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
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
        foreach (WaveSpawner.Wave wave in _waveSpawner.Waves)
        {
            SpawnedBotsForWave[wave] = new List<BaseEnemy>();
        }

        // Create object pool for the wave
        for (int i = 0; i < _waveSpawner.Waves.Length; i++)
        {
            WaveSpawner.Wave wave = _waveSpawner.Waves[i];
            if (wave.Bot == null)
            {
                Debug.LogWarning("No bots found for wave " + wave.Name);
                continue;
            }

            // Add the bots to the List
            for (int j = 0; j < wave.SpawnLimit * 2; j++)
            {
                IEnemy bot = wave.Bot.GetComponent<IEnemy>();
                bot.MeshRenderer.sharedMaterial = wave._objMaterial;
                SpawnedBotsForWave[wave].Add(bot.BaseEnemy);
            }
        }
        List<BaseEnemy> allBots = SpawnedBotsForWave.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseEnemy>.CreateInstance(allBots, gameObject.transform, "Bots", _diContainer);
        _botPool = PoolObject<BaseEnemy>.Instance;
    }
    public async UniTask SpawnObjects(WaveSpawner.Wave wave)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        if (SpawnedBotsForWave.ContainsKey(wave))
        {
            List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];

            for (int j = 0; j < wave.SpawnLimit; j++)
            {
                // Check if there are any inactive bot objects in the pool that match the current wave and prefab

                wave.SpawnMethod.NewUnitCircle();
                wave.SpawnMethod.SpawnPrefabs();
                wave.SpawnMethod.GroundCheck();

                IEnemy inactiveBot = GetInactiveBot(wave);

                if (inactiveBot != null)
                {
                    _objectCreated?.Invoke(inactiveBot.BaseEnemy.gameObject);

                    if (wave.SpawnMethod.ColliderCheck(inactiveBot.BaseEnemy))
                    {

                        _setObjectActive?.Invoke(inactiveBot.BaseEnemy, true);
                        await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                    }
                    else
                    {
                        _botPool.ReturnObject(inactiveBot.BaseEnemy);
                    }
                }
            }
        }
    }

    //public IEnumerator SpawnObjects(WaveSpawner.Wave wave)
    //{
    //    yield return new WaitForSeconds(0.05f);
    //    List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];

    //    for (int j = 0; j < wave.SpawnLimit; j++)
    //    {
    //        // Check if there are any inactive bot objects in the pool that match the current wave and prefab

    //        wave.SpawnMethod.NewUnitCircle();
    //        wave.SpawnMethod.SpawnPrefabs();
    //        wave.SpawnMethod.GroundCheck();

    //        IEnemy inactiveBot = GetInactiveBot(wave);

    //        if(inactiveBot != null)
    //        {
    //            _objectCreated?.Invoke(inactiveBot.BaseEnemy.gameObject);

    //            if (wave.SpawnMethod.ColliderCheck(inactiveBot.BaseEnemy))
    //            {

    //                _setObjectActive?.Invoke(inactiveBot.BaseEnemy, true);
    //                yield return new WaitForSeconds(0.05f);
    //            }
    //            else
    //            {
    //                _botPool.ReturnObject(inactiveBot.BaseEnemy);
    //            }
    //        }
    //    }
    //}

    private IEnemy GetInactiveBot(WaveSpawner.Wave wave)
    {
        List<BaseEnemy> botsForWave = SpawnedBotsForWave[wave];
        for (int i = 0; i < botsForWave.Count; i++)
        {
            IEnemy bot = botsForWave[i];

            if (bot.BaseEnemy.gameObject.activeSelf)
            {
                IEnemy inactiveBot = _botPool.GetObjects(Vector3.zero, bot, _autoExpand);
                return inactiveBot;
            }
        }
        return null;
    }
}
