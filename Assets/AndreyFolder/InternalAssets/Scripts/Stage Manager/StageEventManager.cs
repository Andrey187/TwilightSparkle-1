using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageEventManager : MonoCache
{
    [SerializeField] private StageData _stageData;
    [SerializeField] private BotsSpawn _botsSpawn;

    private StageTime _stageTime; //timer before wave starts
    public int _eventIndexer;

    private void Awake()
    {
        _stageTime = Get<StageTime>();
        for(int i = 0; i < _stageData.ListStageEvent.Count; i++)
        {
            _stageData.ListStageEvent[i].EnemyCount = 0;
            //Debug.Log(_stageData.ListStageEvent[i].EnemyCount);
        }
    }

    protected override void OnEnabled()
    {
        BotsLifeTime.onBotDestroy += Foo;
    }

    protected override void OnDisabled()
    {
        BotsLifeTime.onBotDestroy -= Foo;
    }


    protected override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_eventIndexer >= _stageData.ListStageEvent.Count)
        {
            //Debug.Log("ALLDONE");
            _eventIndexer = 1;
            return;
        }

        else if (_stageTime.Timer >= _stageData.ListStageEvent[_eventIndexer].SpawnInterval)
        {
            _stageTime.Timer = 0f;

            for (int i = 0; i < _stageData.ListStageEvent[_eventIndexer].SpawnCount; i++)
            {
                
                Debug.Log(_stageData.ListStageEvent[_eventIndexer]+ " manager prefab");

            }
            Debug.Log(_eventIndexer);
            _botsSpawn.StartCoroutine(_botsSpawn.SpawnEnemies(_stageData.ListStageEvent[_eventIndexer]));
            
            _eventIndexer += 1;

            //Debug.Log(_eventIndexer + " _eventIndexer");
        }
    }

    //protected override void Run()
    //{
    //    if (_eventIndexer >= _stageData.ListStageEvent.Count) { _eventIndexer = 0; }

    //    if (_stageTime.Timer >= _stageData.ListStageEvent[_eventIndexer].SpawnInterval)
    //    {
    //        _stageTime.Timer = 0f;

    //        for (int i = 0; i < _stageData.ListStageEvent[_eventIndexer].SpawnCount; i++)
    //        {
    //            _botsSpawn.StartCoroutine(_botsSpawn.SpawnEnemies(_stageData.ListStageEvent[_eventIndexer]));
    //            Debug.Log(_stageData.ListStageEvent[_eventIndexer].Type + " manager prefab");
    //        }

    //        _eventIndexer += 1;

    //        Debug.Log(_eventIndexer + " _eventIndexer");
    //    }
    //}



    private void Foo(StageEvent.ObjectType type)
    {
        _stageData.ListStageEvent.Find(x => x.Type == type).EnemyCount--;
    }
}
