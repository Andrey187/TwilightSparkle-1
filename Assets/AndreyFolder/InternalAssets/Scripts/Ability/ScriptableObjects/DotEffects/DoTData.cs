using UnityEngine;

[CreateAssetMenu(menuName = "DoTs/DoTData")]
public class DoTData : ScriptableObject
{
    public string DoTName;
    public float Duration;
    public float TickInterval;
    public Color DoTColor;
}
