using UnityEngine;

public struct ActiveDoT
{
    public IDoTEffect DoTEffect { get; }
    public BaseEnemy Target { get; }
    public int Amount { get; }
    private float timer;

    public ActiveDoT(IDoTEffect doTEffect, BaseEnemy target, int amount)
    {
        DoTEffect = doTEffect;
        Target = target;
        Amount = amount;
        timer = doTEffect.Duration;
    }

    public bool UpdateTimer(float deltaTime)
    {
        timer -= deltaTime;
        return timer <= 0;
    }
}
