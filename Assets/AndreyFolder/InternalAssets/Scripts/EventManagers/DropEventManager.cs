using System;
using UnityEngine;

public class DropEventManager : MonoBehaviour
{
    private Action<GameObject> _dropCreated;

    private static DropEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("DropEventManager");
            _instance = obj.AddComponent<DropEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static DropEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DropEventManager>();
            }

            return _instance;
        }
    }

    public event Action<GameObject> DropCreated
    {
        add { _dropCreated += value; }
        remove { _dropCreated -= value; }
    }

    public void DropsCreated(GameObject obj)
    {
        _dropCreated?.Invoke(obj);
    }
}
