using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Database", menuName = "Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    [SerializeField] public List<AbilityData> AbilityDataList = new List<AbilityData>();
}

