public interface IGamePause
{
    public bool IsPaused { get; }
    public bool IsActiveLines { get; }

    public void SetPause(bool isEnable);

    public void SetActiveLines(bool isEnable);
}
