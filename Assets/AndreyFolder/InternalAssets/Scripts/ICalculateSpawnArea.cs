using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICalculateSpawnArea
{
    public float CircleOutsideTheCameraField { get;}

    public Vector3 SpawnCircleRadius { get;}
    public Vector3 RandomInsideUnitCircle { get;}
    public Vector3 BotsSpawnInRandomPointOnCircle { get;}
    public Vector3 BotsSpawnField { get;}
}
