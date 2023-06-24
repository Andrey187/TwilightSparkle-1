using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject, IResetOnExitPlay, IMagicPowerObserver
{
    public string AbilityName;
    public int Damage;
    public float FireInterval;
    public Color Color;
    public bool HasDoT;

    public int _baseDamage;
    [SerializeField] private float _baseMagicPower;
    [SerializeField] private float _baseFireInterval;
    [SerializeField] private float _currentMagicPower;

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
        Damage = _baseDamage;
        FireInterval = _baseFireInterval;
        _currentMagicPower = _baseMagicPower;
    }
}
