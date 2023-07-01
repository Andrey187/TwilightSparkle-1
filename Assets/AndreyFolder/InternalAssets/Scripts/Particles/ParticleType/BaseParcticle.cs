using UnityEngine;

public abstract class BaseParcticle : MonoBehaviour
{
    [SerializeField] protected internal ParticleData ParticleData;

    [SerializeField] protected internal ParticleData.ParticleType ParticleType;

}
