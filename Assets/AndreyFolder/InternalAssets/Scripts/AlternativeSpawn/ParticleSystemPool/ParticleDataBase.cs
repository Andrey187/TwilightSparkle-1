using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Particle Database", menuName = "Particle Database")]
public class ParticleDataBase : ScriptableObject
{
    [SerializeField] public List<ParticleData> ParticleDataList = new List<ParticleData>();
}
