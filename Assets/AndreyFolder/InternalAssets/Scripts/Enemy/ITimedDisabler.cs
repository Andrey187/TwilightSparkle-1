using System;

public interface ITimedDisabler
{
    public float Timer { get; set; }

    public event Action OnTimerElapsed;

    public void StartTimer();

    public void StopTimer();
}
