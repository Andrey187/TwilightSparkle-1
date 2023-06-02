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
        foreach(var a in Waves)
        {
            StartCoroutine(SpawnWave(a));
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        float timeAccumulator = 0f;

        while (true)
        {
            if (timeAccumulator >= wave.WaveDuration)
            {
                yield return StartCoroutine(_botSpawner.SpawnObjects(wave));
                timeAccumulator = 0f;
            }

            timeAccumulator += Time.deltaTime;
            yield return null;
        }
    }
}