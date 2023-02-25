using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public struct StageEvent
{
    public float Time;
    public string Message;
    public GameObject Prefab;
    public int Count;

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
    public List<StageEvent> StageEvent;
}
