using UnityEngine;

public class FireBallAbility: IAbility
{
    private  AbilityData _abilityData;

    public FireBallAbility()
    {
        _abilityData = DataLoader.Instance.GetAbilityData("FireBall");
    }

    public int Damage { get => _abilityData.Damage; }
    public bool HasDoT { get => _abilityData.HasDoT; }
    public Color Color { get => _abilityData.Color;}

    public int ApplyDamage(int currentHealth, int amount)
    {
        currentHealth -= amount;
        return currentHealth;
    }
}
