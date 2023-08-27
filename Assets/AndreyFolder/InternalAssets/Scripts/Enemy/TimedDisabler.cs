using System;
using UnityEngine;

public class TimedDisabler : MonoCache, ITimedDisabler
{
    [SerializeField] private float _timerDuration = 8f;
    [SerializeField] private bool _isEnabled = true;
    private float _timer = 0f;
    private bool _shouldRunTimer = false;

    public float Timer { get => _timer; set => _timer = value; }

    public event Action OnTimerElapsed;

    protected override void OnEnabled()
    {
        StartTimer();
    }

    protected override void OnDisabled()
    {
        StopTimer();
    }

    public void StartTimer()
    {
        _shouldRunTimer = true;
        _timer = 0f;
    }

    public void StopTimer()
    {
        _shouldRunTimer = false;
    }

    protected override void Run()
    {
        if (_shouldRunTimer && _isEnabled)
        {
            _timer += Time.deltaTime;

            if (_timer >= _timerDuration)
            {
                OnTimerElapsed?.Invoke();
            }
        }
    }
}
