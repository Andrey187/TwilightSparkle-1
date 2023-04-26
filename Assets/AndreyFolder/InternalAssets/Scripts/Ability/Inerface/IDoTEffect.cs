using UnityEngine;

public interface IDoTEffect
{
    // The duration of the effect in seconds
    public float Duration { get; }

    // The time between each tick of damage
    public float TickInterval { get; }

    //Color Dot Effect
    public Color DoTColor { get; }

    // Applies the effect to the given target
    public int ApplyDoT(int currentHealth, int amount);

}
