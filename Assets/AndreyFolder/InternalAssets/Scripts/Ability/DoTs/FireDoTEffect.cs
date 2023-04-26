using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDoTEffect : IDoTEffect
{
    private readonly DoTData _dotData;

    public FireDoTEffect()
    {
        _dotData = DataLoader.Instance.GetDoTData("FireDoT");
    }

    public float Duration => _dotData.Duration;

    public float TickInterval => _dotData.TickInterval;

    public Color DoTColor => _dotData.DoTColor;

    public int ApplyDoT(int currentHealth, int amount)
    {
        currentHealth = amount / 2;
        return currentHealth;
    }
}
