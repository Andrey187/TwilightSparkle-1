using System;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    private Action _abilityChoice;
    private Action<GameObject> _healthBarCreated;
    private Action<GameObject> _healthBarDestroyed;

    private static UIEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("UIEventManager");
            _instance = obj.AddComponent<UIEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static UIEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIEventManager>();
            }

            return _instance;
        }
    }

    public event Action AbilityChoice
    {
        add { _abilityChoice += value; }
        remove { _abilityChoice -= value; }
    }

    public event Action<GameObject> HealthBarCreated
    {
        add { _healthBarCreated += value; }
        remove { _healthBarCreated -= value; }
    }

    public event Action<GameObject> HealthBarDestroyed
    {
        add { _healthBarDestroyed += value; }
        remove { _healthBarDestroyed -= value; }
    }

    public void AbilityChoiceUI()
    {
        _abilityChoice?.Invoke();
    }

    public void CreateHealthBar(GameObject obj)
    {
        _healthBarCreated?.Invoke(obj);
    }

    public void DestroyHealthBar(GameObject obj)
    {
        _healthBarDestroyed?.Invoke(obj);
    }
}
