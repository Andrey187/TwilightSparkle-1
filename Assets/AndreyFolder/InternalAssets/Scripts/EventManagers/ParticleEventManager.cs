using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEventManager : MonoBehaviour
{
    private Action<GameObject> _deathParticle;
    private Action<GameObject> _healParticle;

    private static ParticleEventManager _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("ParticleEventManager");
            _instance = obj.AddComponent<ParticleEventManager>();
            DontDestroyOnLoad(obj);
        }
    }

    public static ParticleEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticleEventManager>();
            }

            return _instance;
        }
    }

    public event Action<GameObject> OnDeathParticleSetActive
    {
        add { _deathParticle += value; }
        remove { _deathParticle -= value; }
    }

    public event Action<GameObject> OnHealParticleSetActive
    {
        add { _healParticle += value; }
        remove { _healParticle -= value; }
    }

    public void DeathParticle(GameObject obj)
    {
        _deathParticle?.Invoke(obj);
    }

    public void HealParticle(GameObject obj)
    {
        _healParticle?.Invoke(obj);
    }
}
