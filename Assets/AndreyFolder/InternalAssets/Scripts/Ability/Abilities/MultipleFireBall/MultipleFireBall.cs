using UnityEngine;

public class MultipleFireBall : IAbility
{
    private static AbilityData _abilityData;

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

    public MultipleFireBall()
    {
        _abilityData = DataLoader.Instance.GetAbilityData("MultipleFireBall");
    }

    private static MultipleFireBall _instance;
    public static MultipleFireBall Instance
    {
        get
        {
            if (_instance == null)

                _instance = new MultipleFireBall();
            return _instance;
        }
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
