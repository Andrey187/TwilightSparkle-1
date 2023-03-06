using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class StageEvent
{
    public float SpawnInterval;
    public string Message;
    public GameObject Prefab;
    public int SpawnCount;
    public int EnemyCount;

    public enum ObjectType
    {
        ENEMY_1,
        ENEMY_2,
        ENEMY_3,
        ENEMY_4,
    }

    public ObjectType Type;
}


[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public List<StageEvent> ListStageEvent;
}
