using UnityEngine;

[CreateAssetMenu(fileName = "New Particle Data", menuName = "Particle Data")]
public class ParticleData : ScriptableObject
{
    public string ParticleName;

    public ParticleType Type;

    public enum ParticleType
    {
        Death,
        Hit,
        Heal,
        Portal
    }
}
