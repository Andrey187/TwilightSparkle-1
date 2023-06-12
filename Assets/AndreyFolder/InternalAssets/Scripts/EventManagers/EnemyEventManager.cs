using System;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    private Action<GameObject> _onObjectSetActive;//For check object active or enable
    private Action<GameObject> _objectCreated;
    private Action<GameObject> _objectDestroyed;

    private static EnemyEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("EnemyEventManager");
            _instance = obj.AddComponent<EnemyEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static EnemyEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyEventManager>();
            }

            return _instance;
        }
    }

    public event Action<GameObject> OnObjectSetActive
    {
        add { _onObjectSetActive += value; }
        remove { _onObjectSetActive -= value; }
    }

    public event Action<GameObject> ObjectCreated
    {
        add { _objectCreated += value; }
        remove { _objectCreated -= value; }
    }

    public event Action<GameObject> ObjectDestroyed
    {
        add { _objectDestroyed += value; }
        remove { _objectDestroyed -= value; }
    }

    public void SetObjectActive(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
        _onObjectSetActive?.Invoke(obj);
    }
    public void CreatedObject(GameObject obj)
    {
        _objectCreated?.Invoke(obj);
    }

    public void DestroyedObject(GameObject obj)
    {
        _objectDestroyed?.Invoke(obj);
    }
}
