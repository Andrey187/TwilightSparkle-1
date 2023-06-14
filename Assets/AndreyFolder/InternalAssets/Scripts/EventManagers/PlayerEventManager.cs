using System;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    private Action _playerDeath;
    private Action<int> _playerLevelUP;

    private static PlayerEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("PlayerEventManager");
            _instance = obj.AddComponent<PlayerEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static PlayerEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerEventManager>();
            }

            return _instance;
        }
    }

    public event Action PlayerDeath
    {
        add { _playerDeath += value; }
        remove { _playerDeath -= value; }
    }

    public event Action<int> PlayerLevelUp
    {
        add { _playerLevelUP += value; }
        remove { _playerLevelUP -= value; }
    }

    public void PlayerDie()
    {
        _playerDeath?.Invoke();
    }

    public void PlayerLevelChanged(int newLevel)
    {
        _playerLevelUP?.Invoke(newLevel);
    }
}
