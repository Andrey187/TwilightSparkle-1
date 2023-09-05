using UnityEngine;

public struct ActiveDoT
{
    public IDoTEffect DoTEffect { get; }
    public BaseEnemy Target { get; }
    public int Amount { get; }

    public float Timer { get; set; }

    public float Interval { get; set; }

    public ActiveDoT(IDoTEffect doTEffect, BaseEnemy target, int amount)
    {
        DoTEffect = doTEffect;
        Target = target;
        Amount = amount;
        Timer = doTEffect.Duration;
        Interval = doTEffect.TickInterval;
    }
}
