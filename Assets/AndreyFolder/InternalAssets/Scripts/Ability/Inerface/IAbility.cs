using UnityEngine;

public interface IAbility
{
    public int Damage { get; }

    public bool HasDoT { get; }

    public Color Color { get; }

    public int ApplyDamage(int currentHealth, int amount);
}
