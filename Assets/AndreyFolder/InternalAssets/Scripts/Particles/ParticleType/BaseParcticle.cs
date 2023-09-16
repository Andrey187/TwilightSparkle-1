using System;
using UnityEngine;

public abstract class BaseParcticle : MonoBehaviour
{
    [SerializeField] protected internal ParticleData ParticleData;

    [SerializeField] protected internal ParticleData.ParticleType ParticleType;

    protected internal Type GetParticleType(ParticleData.ParticleType particleType)
    {
        switch (particleType)
        {
            case ParticleData.ParticleType.Death:
                return typeof(ParticleDeath);
            case ParticleData.ParticleType.Heal:
                return typeof(ParticleHeal);
            case ParticleData.ParticleType.Portal:
                return typeof(ParticlePortal);
            default:
                throw new ArgumentException("Unknown particle type: " + particleType);
        }
    }
}
