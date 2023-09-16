using System;
using UnityEngine;

public class InvokePortal : MonoCache
{
    private event Action<GameObject> _portalParticleFromPoolDelegate;

    private void Awake()
    {
        _portalParticleFromPoolDelegate += ParticleEventManager.Instance.PortalParticle;
    }

    protected override void OnEnabled()
    {
        Invoke("SetActivePortal", 0.01f);
    }

    protected override void OnDisabled()
    {
        SetActivePortal();
    }

    private void SetActivePortal()
    {
        _portalParticleFromPoolDelegate?.Invoke(gameObject);
    }
}
