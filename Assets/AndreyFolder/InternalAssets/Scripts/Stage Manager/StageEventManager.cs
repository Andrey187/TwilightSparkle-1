using UnityEngine;

public class StageEventManager : MonoCache
{
    [SerializeField] private StageData _stageData;
    [SerializeField] private BotsSpawn _botsSpawn;

    private StageTime _stageTime; //timer before wave starts
    private int _eventIndexer;

    private void Awake()
    {
        _stageTime = Get<StageTime>();
    }

    protected override void Run()
    {
        if(_eventIndexer >= _stageData.StageEvent.Count) { return; }
        if(_stageTime.Timer > _stageData.StageEvent[_eventIndexer].Time)
        {
            for(int i=0; i<_stageData.StageEvent[_eventIndexer].Count; i++)
            {
                _botsSpawn.StartCoroutine(_botsSpawn.SpawnEnemies());
            }

            _eventIndexer += 1;
        }
    }
}
