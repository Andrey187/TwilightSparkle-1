using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject, IResetOnExitPlay, IMagicPowerObserver
{
    public string AbilityName;
    public int Damage;
    public float FireInterval;
    public Color Color;
    public bool HasDoT;
    [SerializeField] private float _currentMagicPower;

    [Header("Default Settings")]
    [SerializeField] private int _DefaultDamage;
    [SerializeField] private float _DefaultFireInterval;
    [SerializeField] private float _DefaultMagicPower;

    public void OnMagicPowerChanged(float newMagicPower)
    {
        _currentMagicPower = newMagicPower;
        RecalculateDamage();
    }

    private void RecalculateDamage()
    {
        Damage = Damage + Mathf.RoundToInt(Damage * _currentMagicPower / 100f);
    }

    public void ResetOnExitPlay()
    {
        Damage = _DefaultDamage;
        FireInterval = _DefaultFireInterval;
        _currentMagicPower = _DefaultMagicPower;
    }
}
