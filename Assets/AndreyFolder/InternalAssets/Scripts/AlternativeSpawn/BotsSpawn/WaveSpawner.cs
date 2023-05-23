using System.Collections;
using UnityEngine;
using System;

public class WaveSpawner : MonoCache
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Serializable]
    public class Wave
    {
        public string Name;
        public Transform Bot;
        public int SpawnLimit;
    }

    [SerializeField] private BotsSpawner _botSpawner;
    public Wave[] Waves;
    public float TimeBtwWaves = 5f;
    public float WaveCountdown;

    private int _nextWave = 0;
    private float searchCountDown = 1f;
    private SpawnState State = SpawnState.COUNTING;

    private void Start()
    {
        WaveCountdown = TimeBtwWaves;
    }

    protected override void Run()
    {
        if (State == SpawnState.WAITING)
        {
            if (!BotsIsAlive())
            {
                WaveCompleted();
            }
            else { return; }
        }

        if (WaveCountdown <= 0)
        {
            if (State != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(Waves[_nextWave]));
            }
        }
        else { WaveCountdown -= Time.deltaTime; }
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        State = SpawnState.COUNTING;
        WaveCountdown = TimeBtwWaves;

        if (_nextWave + 1 > Waves.Length - 1)
        {
            _nextWave = 0;
            Debug.Log("All WAVES COMPLETED! Looping...");
        }
        else { _nextWave++; }
    }

    private bool BotsIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        State = SpawnState.SPAWNING;

        StartCoroutine(_botSpawner.SpawnObjects(_wave));

        State = SpawnState.WAITING;
        yield break;
    }
}