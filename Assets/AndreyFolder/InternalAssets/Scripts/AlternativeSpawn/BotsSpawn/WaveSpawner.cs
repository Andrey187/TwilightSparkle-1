using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

public class WaveSpawner : MonoCache, IWaveSpawner
{
    [Serializable]
    public class Wave
    {
        public string Name;
        public Transform Bot;
        public int SpawnLimit;
        public float WaveDuration;
        public float BaseDuration;
        public SpawnMethod SpawnMethod;
        public Material _objMaterial;
    }

    [SerializeField] private BotsSpawner _botSpawner;
    [SerializeField] private int _startWaveIndex;
    [SerializeField] private Wave[] _waves;
    private CancellationTokenSource _cancellationTokenSource;
    [Inject] private IGamePause _gamePause;

    public Wave[] Waves => _waves;

    public CancellationTokenSource CancellationTokenSource => _cancellationTokenSource;

    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        foreach (Wave wave in _waves)
        {
            wave.WaveDuration = wave.BaseDuration;
            wave._objMaterial = Instantiate(wave.Bot.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }

        // Создаем новый токен отмены для этой волны
        _cancellationTokenSource = new CancellationTokenSource();

        for (int i = _startWaveIndex; i < _waves.Length; i++) // Start from index 1 for subsequent waves
        {
            // Запускаем SpawnWave с токеном отмены
            _ = SpawnWave(_waves[i], _cancellationTokenSource.Token);

        }
    }

    private async UniTask SpawnWave(Wave wave, CancellationToken cancellationToken)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(wave.WaveDuration), false, PlayerLoopTiming.Update, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                // Остановка выполнения метода, если токен отмены активирован
                break;
            }

            if (!_gamePause.IsPaused) // Проверяем, не установлена ли пауза
            {
                _ = _botSpawner.SpawnObjects(wave);
            }
        }
    }

    public async void BossSpawn(Wave wave)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        _ = _botSpawner.SpawnObjects(wave);
    }
}