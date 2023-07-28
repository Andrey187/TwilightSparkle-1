using System.Collections;
using UnityEngine;
using System;

public class WaveSpawnerJob : MonoCache
{
    [Serializable]
    public class WaveJob
    {
        public string Name;
        public Transform Bot;
        public int SpawnLimit;
        public float WaveDuration;
        public float BaseDuration;
        public EnemySpawnMethodJob SpawnMethod;
        public Material _objMaterial;
    }

    [SerializeField] private BotsSpawnerJob _botSpawnerJob;
    [SerializeField] private int _startWaveIndex;
    public WaveJob[] Waves;

    private void Start()
    {
        foreach (WaveJob wave in Waves)
        {
            wave.WaveDuration = wave.BaseDuration;
            wave._objMaterial = Instantiate(wave.Bot.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }

        //StartCoroutine(SpawnFirstWave()); // Start the first wave immediately

        for (int i = _startWaveIndex; i < Waves.Length; i++) // Start from index 1 for subsequent waves
        {
            //StopCoroutine(SpawnFirstWave());
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

    private IEnumerator SpawnWave(WaveJob wave, bool isFirstWave)
    {
        if (!isFirstWave)
        {
            yield return new WaitForSeconds(wave.WaveDuration); // Wait for the specified WaveDuration before spawning the wave
        }

        while (true)
        {
            yield return StartCoroutine(_botSpawnerJob.SpawnObjects(wave));
            yield return new WaitForSeconds(wave.WaveDuration); // Wait for the specified WaveDuration before spawning the next wave
        }
    }
    private IEnumerator SpawnFirstWave()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before spawning the first wave
        StartCoroutine(SpawnWave(Waves[0], true)); // Start the first wave
    }

    public void BossSpawn(WaveJob wave)
    {
        StartCoroutine(_botSpawnerJob.SpawnObjects(wave));
    }
}
