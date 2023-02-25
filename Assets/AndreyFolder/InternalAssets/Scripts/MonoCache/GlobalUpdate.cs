using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class GlobalUpdate : Singleton<GlobalUpdate>
{
    public const string OnEnableMethodName = "OnEnable";
    public const string OnDisableMethodName = "OnDisable";

    public const string UpdateMethodName = nameof(Update);
    public const string FixedUpdateMethodName = nameof(FixedUpdate);
    public const string LateUpdateMethodName = nameof(LateUpdate);

    private readonly List<IRunSystem> _runSystems = new List<IRunSystem>(1024);
    private readonly List<IFixedRunSystem> _fixedRunSystems = new List<IFixedRunSystem>(512);
    private readonly List<ILateRunSystem> _lateRunSystems = new List<ILateRunSystem>(256);

    private readonly MonoCacheExceptionsChecker _monoCacheExceptionsChecker =
        new MonoCacheExceptionsChecker();

    private void Awake()
    {
        _monoCacheExceptionsChecker.CheckForExceptions();
    }

    public void AddRunSystem(IRunSystem runSystem)
    {
        _runSystems.Add(runSystem);
    }

    public void AddFixedRunSystem(IFixedRunSystem fixedRunSystem)
    {
        _fixedRunSystems.Add(fixedRunSystem);
    }

    public void AddLateRunSystem(ILateRunSystem lateRunSystem)
    {
        _lateRunSystems.Add(lateRunSystem);
    }

    public void RemoveRunSystem(IRunSystem runSystem)
    {
        _runSystems.Remove(runSystem);
    }

    public void RemoveFixedRunSystem(IFixedRunSystem fixedRunSystem)
    {
        _fixedRunSystems.Remove(fixedRunSystem);
    }

    public void RemoveLateRunSystem(ILateRunSystem lateRunSystem)
    {
        _lateRunSystems.Remove(lateRunSystem);
    }

    private void Update()
    {
        for (int i = 0; i < _runSystems.Count; i++)
        {
            _runSystems[i].OnRun();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _fixedRunSystems.Count; i++)
        {
            _fixedRunSystems[i].OnFixedRun();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _lateRunSystems.Count; i++)
        {
            _lateRunSystems[i].OnLateRun();
        }
    }
}
