using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[BurstCompile]
public struct SpawnJob : IJobParallelFor
{
    [ReadOnly] public float spawnRadius;
    [ReadOnly] public float groupSpawnCircleRadius;
    [ReadOnly] public float distanceToCheckGround;
    [ReadOnly] public float colliderHitRadius;
    public float3 playerPos;
    public NativeArray<float3> randomInsideUnitCircle;
    public NativeArray<float3> spawnCircleRadius;
    public NativeArray<float3> botsSpawnInRandomPointOnCircle;
    public NativeArray<float3> botsSpawnField;
    public NativeArray<float3> Pos;
    public NativeArray<float2> randomInsideUnitCircle2D;
    public RaycastHit hit;
    public bool isGround;

    public void Execute(int i)
    {
        spawnCircleRadius[i] = playerPos + randomInsideUnitCircle[i] * spawnRadius;
        botsSpawnInRandomPointOnCircle[i] = new float3(spawnCircleRadius[i].x,
            0,
            spawnCircleRadius[i].z);

        //Pos[i] = new float3(randomInsideUnitCircle2D[i].x, 0f, randomInsideUnitCircle2D[i].y);

        //botsSpawnField[i] = botsSpawnInRandomPointOnCircle[i] +
        //    Pos[i]
        //    * groupSpawnCircleRadius;

    }
}
