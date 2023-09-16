using System;
using UnityEngine;

public class ParticleEventManager : MonoBehaviour
{
    private Action<GameObject, ParticleData.ParticleType> _deathParticle;
    private Action<GameObject, ParticleData.ParticleType> _healParticle;
    private Action<GameObject, ParticleData.ParticleType> _portalParticle;

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

    public event Action<GameObject, ParticleData.ParticleType> OnDeathParticleSetActive
    {
        add { _deathParticle += value; }
        remove { _deathParticle -= value; }
    }

    public event Action<GameObject, ParticleData.ParticleType> OnHealParticleSetActive
    {
        add { _healParticle += value; }
        remove { _healParticle -= value; }
    }

    public event Action<GameObject, ParticleData.ParticleType> OnParticlePortalSetActive
    {
        add { _portalParticle += value; }
        remove { _portalParticle -= value; }
    }

    public void DeathParticle(GameObject obj)
    {
        _deathParticle?.Invoke(obj, ParticleData.ParticleType.Death);
    }

    public void HealParticle(GameObject obj)
    {
        _healParticle?.Invoke(obj, ParticleData.ParticleType.Heal);
    }

    public void PortalParticle(GameObject obj)
    {
        _portalParticle?.Invoke(obj, ParticleData.ParticleType.Portal);
    }
}
