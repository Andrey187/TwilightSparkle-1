using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dot Database", menuName = "Dot Database")]
public class DoTDataBase : ScriptableObject
{
    [SerializeField] public List<DoTData> DoTDataList = new List<DoTData>();
}
