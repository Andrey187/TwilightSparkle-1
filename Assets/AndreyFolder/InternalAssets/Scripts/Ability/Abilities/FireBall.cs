using UnityEngine;

public class FireBall : IAbility
{
    private readonly AbilityData _abilityData;

    private IAbility _currentAbility;
    public IAbility CurrentAbility
    {
        get => _currentAbility;
        set => _currentAbility = value;
    }

    private IDoTEffect _doTEffect;
    public IDoTEffect DoTEffect
    {
        get => _doTEffect;
        set => _doTEffect = value;
    }

    public FireBall()
    {
        _abilityData = DataLoader.Instance.GetAbilityData("FireBall");
    }

    public int Damage { get => _abilityData.Damage; }
    public bool HasDoT { get => _abilityData.HasDoT; }
    public Color Color { get => _abilityData.Color; }

    public int ApplyDamage(int currentHealth, int amount)
    {
        currentHealth -= amount;
        return currentHealth;
    }
}

