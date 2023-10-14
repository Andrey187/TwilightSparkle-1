using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BaseBotsMoveable : MonoCache, IEnemyMove
{
    [SerializeField] protected internal Position _targetPosition;
    [SerializeField] protected internal NavMeshAgent _navMeshAgent;
    [Inject] protected IGamePause _gamePause;
    protected internal IEnemy _baseEnemy;
    protected float _lastExecutionTime;
    protected bool _navMeshParamsSet = true;

    NavMeshAgent IEnemyMove.NavMeshAgent { get => _navMeshAgent; }

    protected virtual void Awake()
    {
        _navMeshAgent = Get<NavMeshAgent>();
        _baseEnemy = Get<BaseEnemy>();
    }

    protected override void OnEnabled()
    {
        _navMeshAgent.enabled = true;
    }

    protected override void OnDisabled()
    {
        NavMeshParams();
    }

    protected override void Run()
    {
        // Start measuring the CPU usage for the Update method
        UnityEngine.Profiling.Profiler.BeginSample("UpdateEnemyMove");

        if (_gamePause.IsPaused)
        {
            _navMeshAgent.speed = 0;
            _navMeshParamsSet = false;
            return;
        }
        else
        {
            if (!_navMeshParamsSet)
            {
                NavMeshParams();
                _navMeshParamsSet = true;
            }
        }

        if (gameObject.activeSelf && _navMeshAgent.isOnNavMesh)
        {
            float currentTime = Time.time;
            if (currentTime - _lastExecutionTime >= 0.6f)
            {
                _lastExecutionTime = currentTime;
                _navMeshAgent.destination = _targetPosition.Value;
                _baseEnemy.MeshAnimator.Play("Run");
            }
        }

        // End the measurement
        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected void NavMeshParams()
    {
        _navMeshAgent.speed = _baseEnemy.EnemyType.Speed;
    }
}

//using UnityEngine;
//using Unity.Jobs;
//using Unity.Collections;
//using Unity.Burst;
//using UnityEngine.AI;
//using Unity.Mathematics;
//using System.Collections.Generic;
//using System;

//[BurstCompile]
//public struct MoveBotsJob : IJobParallelFor
//{
//    public NativeArray<Vector3> targetPositions;
//    public NativeArray<float3> navMeshAgentDestinations;

//    public void Execute(int index)
//    {
//        navMeshAgentDestinations[index] = targetPositions[index];
//    }
//}

//public class BaseBotsMoveable : MonoCache
//{
//    [SerializeField] protected internal List<Position> _targetPositions;
//    private NavMeshAgent[] _navMeshAgents;
//    private BaseEnemy[] _baseEnemies;

//    private NativeArray<Vector3> _nativeTargetPositions;
//    private NativeArray<float3> _nativeNavMeshAgentDestinations;

//    protected virtual void Awake()
//    {
//        _navMeshAgents = GetComponentsInChildren<NavMeshAgent>();
//        _baseEnemies = GetComponentsInChildren<BaseEnemy>();
//    }

//    private void Start()
//    {
//        GameObject targetObject = GameObject.Find("PolyArtWizardStandardMat");
//        PositionWritter positionWritter = targetObject.transform.GetComponent<PositionWritter>();
//        _targetPositions.Add(positionWritter._position);

//        _nativeTargetPositions = new NativeArray<Vector3>(_targetPositions.Count, Allocator.Persistent);
//        _nativeNavMeshAgentDestinations = new NativeArray<float3>(_navMeshAgents.Length, Allocator.Persistent);
//    }

//    protected override void Run()
//    {
//        for (int i = 0; i < _targetPositions.Count; i++)
//        {
//            _nativeTargetPositions[i] = _targetPositions[i].Value;
//        }

//        MoveBotsJob moveBotsJob = new MoveBotsJob
//        {
//            targetPositions = _nativeTargetPositions,
//            navMeshAgentDestinations = _nativeNavMeshAgentDestinations
//        };

//        int batchCount = Mathf.Max(1, Environment.ProcessorCount);
//        JobHandle jobHandle = moveBotsJob.Schedule(_targetPositions.Count, batchCount);
//        jobHandle.Complete();

//        for (int i = 0; i < _navMeshAgents.Length; i++)
//        {
//            if (_navMeshAgents[i].isOnNavMesh)
//            {
//                _navMeshAgents[i].destination = _nativeNavMeshAgentDestinations[i];
//                _baseEnemies[i].ChangeState(new WalkingState());
//            }
//        }
//    }

//    private void OnDestroy()
//    {
//        _nativeTargetPositions.Dispose();
//        _nativeNavMeshAgentDestinations.Dispose();
//    }
//}
