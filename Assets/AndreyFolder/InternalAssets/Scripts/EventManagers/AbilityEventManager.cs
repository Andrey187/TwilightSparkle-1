using System;
using UnityEngine;

public class AbilityEventManager : MonoBehaviour
{
    private Action<BaseEnemy, int, IAbility, IDoTEffect> _takeAbilityDamage;

    private static AbilityEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("AbilityEventManager");
            _instance = obj.AddComponent<AbilityEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static AbilityEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AbilityEventManager>();
            }

            return _instance;
        }
    }

    public event Action<BaseEnemy, int, IAbility, IDoTEffect> TakeAbilityDamage
    {
        add { _takeAbilityDamage += value; }
        remove { _takeAbilityDamage -= value; }
    }

    public void AbillityDamage(BaseEnemy enemy, int amount, IAbility ability, IDoTEffect doTEffect)
    {
        _takeAbilityDamage?.Invoke(enemy, amount, ability, doTEffect);
    }
}
