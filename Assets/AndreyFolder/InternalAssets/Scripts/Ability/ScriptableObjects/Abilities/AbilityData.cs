using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject, IResetOnExitPlay
{
    public string AbilityName;
    public int Damage;
    public float FireInterval;
    public Color Color;
    public bool HasDoT;

    [SerializeField] private int _baseDamage;
    [SerializeField] private float _baseFireInterval;

    public void ResetOnExitPlay()
    {
        Damage = _baseDamage;
        FireInterval = _baseFireInterval;
    }
}
