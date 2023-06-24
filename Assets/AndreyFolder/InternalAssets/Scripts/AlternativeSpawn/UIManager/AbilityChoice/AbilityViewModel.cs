public class AbilityViewModel
{
    private AbilityData _abilityData;
    private float _fireIntervalDecrease;
    
    public AbilityViewModel(AbilityData abilityData,/* float fireIntervalDecrease,*/ int damageIncrease)
    {
        _abilityData = abilityData;
        //_fireIntervalDecrease = fireIntervalDecrease;
    }

    public void UpgradeAbility(int damage)
    {
        _abilityData.Damage += damage;
        //_abilityData.FireInterval -= _fireIntervalDecrease;
    }
}
