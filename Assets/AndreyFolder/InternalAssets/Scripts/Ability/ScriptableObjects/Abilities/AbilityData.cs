using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string AbilityName;
    public int Damage;
    public Color Color;
    public bool HasDoT;

}
