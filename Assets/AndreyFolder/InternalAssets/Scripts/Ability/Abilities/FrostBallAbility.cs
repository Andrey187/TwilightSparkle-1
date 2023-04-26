using UnityEngine;

public class FrostBallAbility : IAbility
{
    private readonly AbilityData _abilityData;

    public FrostBallAbility()
    {
        _abilityData = DataLoader.Instance.GetAbilityData("FrostBall");
    }
    public int Damage { get => _abilityData.Damage;}
    public bool HasDoT { get => _abilityData.HasDoT;}
    public Color Color { get => _abilityData.Color;}

    public int ApplyDamage(int currentHealth, int amount)
    {
        currentHealth -= amount;
        return currentHealth;
    }
}
