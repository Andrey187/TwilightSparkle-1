using DamageNumber;
using System;
using UnityEngine;

public class TextDamageEvent : MonoBehaviour
{
    private Action<DamageAbilityNumbers> _returnToPoolDamageText;
    private Action<DamageDotNumbers> _returnToPoolDamageDotNumbersText;

    private static TextDamageEvent _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("TextDamageEvent");
            _instance = obj.AddComponent<TextDamageEvent>();
            DontDestroyOnLoad(obj);
        }
    }

    public static TextDamageEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TextDamageEvent>();
            }

            return _instance;
        }
    }

    public event Action<DamageAbilityNumbers> ReturnToPoolEventDamageText
    {
        add { _returnToPoolDamageText += value; }
        remove { _returnToPoolDamageText -= value; }
    }

    public event Action<DamageDotNumbers> ReturnToPoolEventDamageDotNumbersText
    {
        add { _returnToPoolDamageDotNumbersText += value; }
        remove { _returnToPoolDamageDotNumbersText -= value; }
    }

    public void ReturnToPoolDamageText(DamageAbilityNumbers component)
    {
        _returnToPoolDamageText?.Invoke(component);
    }

    public void ReturnToPoolDamageDotNumbersText(DamageDotNumbers component)
    {
        _returnToPoolDamageDotNumbersText?.Invoke(component);
    }
}
