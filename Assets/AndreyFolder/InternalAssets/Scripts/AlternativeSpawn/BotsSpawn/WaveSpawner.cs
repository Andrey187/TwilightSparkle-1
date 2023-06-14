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
        public float BaseDuration;
        public EnemySpawnMethod SpawnMethod;
    }

    [SerializeField] private BotsSpawner _botSpawner;
    public Wave[] Waves;

    private void Start()
    {
        foreach(Wave wave in Waves)
        {
            wave.WaveDuration = wave.BaseDuration;
        }

        StartCoroutine(SpawnFirstWave()); // Start the first wave immediately

        for (int i = 0; i < Waves.Length; i++) // Start from index 1 for subsequent waves
        {
            StopCoroutine(SpawnFirstWave());
            StartCoroutine(SpawnWave(Waves[i], false));
        }
    }

    protected override void OnDisabled()
    {
        for (int i = 0; i < Waves.Length; i++) // Start from index 1 for subsequent waves
        {
            StopCoroutine(SpawnWave(Waves[i], false));
        }
    }

    private IEnumerator SpawnWave(Wave wave, bool isFirstWave)
    {
        if (!isFirstWave)
        {
            yield return new WaitForSeconds(wave.WaveDuration); // Wait for the specified WaveDuration before spawning the wave
        }

        while (true)
        {
            yield return StartCoroutine(_botSpawner.SpawnObjects(wave));
            
            yield return new WaitForSeconds(wave.WaveDuration); // Wait for the specified WaveDuration before spawning the next wave
        }
    }
    private IEnumerator SpawnFirstWave()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before spawning the first wave
        StartCoroutine(SpawnWave(Waves[0], true)); // Start the first wave
    }
}