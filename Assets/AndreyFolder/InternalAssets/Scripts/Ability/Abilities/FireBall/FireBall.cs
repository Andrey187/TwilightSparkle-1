using UnityEngine;

public class FireBall : IAbility
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

    public FireBall()
    {
        _abilityData = DataLoader.Instance.GetAbilityData("FireBall");
    }

    private static FireBall _instance;
    public static FireBall Instance
    {
        get
        {
            if (_instance == null)
            
            _instance = new FireBall();
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

