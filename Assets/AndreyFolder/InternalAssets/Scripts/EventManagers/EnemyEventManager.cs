using System;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    private Action<IEnemy> _onObjectSetActive;//For check object active or enable
    private Action<GameObject> _objectCreated;
    private Action<GameObject> _objectDie;
    private Action _bossDie;

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

    public event Action<IEnemy> OnObjectSetActive
    {
        add { _onObjectSetActive += value; }
        remove { _onObjectSetActive -= value; }
    }

    public event Action<GameObject> ObjectCreated
    {
        add { _objectCreated += value; }
        remove { _objectCreated -= value; }
    }

    public event Action<GameObject> ObjectDie
    {
        add { _objectDie += value; }
        remove { _objectDie -= value; }
    }

    public event Action BossDie
    {
        add { _bossDie += value; }
        remove { _bossDie -= value; }
    }

    public void SetObjectActive(IEnemy obj, bool isActive)
    {
        obj.BaseEnemy.gameObject.SetActive(isActive);
        _onObjectSetActive?.Invoke(obj);
    }
    public void CreatedObject(GameObject obj)
    {
        _objectCreated?.Invoke(obj);
    }

    public void DieObject(GameObject obj)
    {
        _objectDie?.Invoke(obj);
    }

    public void DieBoss()
    {
        _bossDie?.Invoke();
    }
}
