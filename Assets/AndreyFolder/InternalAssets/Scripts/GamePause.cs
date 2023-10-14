using UnityEngine;

public class GamePause : MonoBehaviour, IGamePause
{
    private bool _isPaused = false;
    private bool _isActiveLines = false;
    public bool IsPaused { get => _isPaused;}

    public bool IsActiveLines { get => _isActiveLines; }

    public void SetPause(bool isEnable)
    {
        _isPaused = isEnable;
    }

    public void SetActiveLines(bool isEnable)
    {
        _isActiveLines = isEnable;
    }
}
