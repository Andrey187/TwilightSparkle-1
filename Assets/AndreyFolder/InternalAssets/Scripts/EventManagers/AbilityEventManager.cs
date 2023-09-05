using System;
using UnityEngine;

public class AbilityEventManager : MonoBehaviour
{
    private Action<IEnemy, int, IAbility, IDoTEffect> _takeAbilityDamageIEnemy;

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

    public event Action<IEnemy, int, IAbility, IDoTEffect> TakeAbilityDamageIEnemy
    {
        add { _takeAbilityDamageIEnemy += value; }
        remove { _takeAbilityDamageIEnemy -= value; }
    }

    public void AbillityDamageIEnemy(IEnemy enemy, int amount, IAbility ability, IDoTEffect doTEffect)
    {
        _takeAbilityDamageIEnemy?.Invoke(enemy, amount, ability, doTEffect);
    }
}
