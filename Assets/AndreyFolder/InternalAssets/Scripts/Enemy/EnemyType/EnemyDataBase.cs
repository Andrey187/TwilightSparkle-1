using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Database", menuName = "Enemy Database")]
public class EnemyDataBase : ScriptableObject
{
    [SerializeField] public List<EnemyData> EnemyDataList = new List<EnemyData>();
}
