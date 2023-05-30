using System.Collections;
using UnityEngine;
using System;

public class WaveSpawner : MonoCache
{
    [Serializable]
    public class Wave
    {
        public string Name;
        public Transform Bot;
        public int SpawnLimit;
        public float WaveDuration;
        public EnemySpawnMethod SpawnMethod;
    }

    [SerializeField] private BotsSpawner _botSpawner;
    public Wave[] Waves;

    private void Start()
    {
        for (int i = 0; i < Waves.Length; i++)
        {
            Wave wave = Waves[i];
            StartCoroutine(SpawnWaveWithDelay(wave, wave.WaveDuration));
        }
    }

    private IEnumerator SpawnWaveWithDelay(Wave wave, float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(wave.WaveDuration);
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        yield return StartCoroutine(_botSpawner.SpawnObjects(wave));
    }
}