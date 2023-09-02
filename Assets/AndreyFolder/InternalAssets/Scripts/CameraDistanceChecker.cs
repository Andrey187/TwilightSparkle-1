using System.Threading.Tasks;
using UnityEngine;

public class CameraDistanceChecker : ParamsForCalculateSpawnPositions
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private TimedDisabler _timedDisabler;
    private GameObject _enemyGameObject;
    private float _lastExecutionTime;
    private bool _isActive = false;
    private bool _previousVisibility = false;

    protected override void Start()
    {
        base.Start();
        _enemyGameObject = gameObject;
        _timedDisabler = Get<TimedDisabler>();
    }

    protected override void OnEnabled()
    {
        _isActive = true;
    }

    protected override void OnDisabled()
    {
        _isActive = false;
    }


    protected override void Run()
    {
        UnityEngine.Profiling.Profiler.BeginSample("CameraDistanceChecker");
        float currentTime = Time.time;
        if (currentTime - _lastExecutionTime >= 1f)
        {
            _lastExecutionTime = currentTime;

            // Вычисляем расстояние между _player и image
            float distance = Vector3.Distance(_player.position, gameObject.transform.position);
            bool active = distance <= _circleOutsideTheCameraField;
            if (active != _previousVisibility)
            {
                objectRenderer.enabled = active;
                if (active)
                {
                    _timedDisabler.StopTimer();
                }
                else
                {
                    _timedDisabler.StartTimer();
                }
                _previousVisibility = active;
            }
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }
}
