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
        public Material _objMaterial;
    }

    [SerializeField] private BotsSpawner _botSpawner;
    [SerializeField] private int _startWaveIndex;
    public Wave[] Waves;

    private void Start()
    {
        foreach(Wave wave in Waves)
        {
            wave.WaveDuration = wave.BaseDuration;
            wave._objMaterial = Instantiate(wave.Bot.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }

        Invoke("StartSpawn",2f);
    }

    private void StartSpawn()
    {
        StartCoroutine(SpawnFirstWave()); // Start the first wave immediately

        for (int i = _startWaveIndex; i < Waves.Length; i++) // Start from index 1 for subsequent waves
        {
            StopCoroutine(SpawnFirstWave());
            StartCoroutine(SpawnWave(Waves[i], false));
        }
    }

    protected override void OnDisabled()
    {
        for (int i = 0; i < Waves.Length; i++)
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
        StartCoroutine(SpawnWave(Waves[1], true)); // Start the first wave
    }

    public void BossSpawn(Wave wave)
    {
        StartCoroutine(_botSpawner.SpawnObjects(wave));
    }
}