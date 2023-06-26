public class EnemyTurtle : BaseEnemy
{
    protected override void OnEnabled()
    {
        base.OnEnabled();
        AbilityEventManager.Instance.TakeAbilityDamage += TakeDamage;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        if (AbilityEventManager.Instance != null)
        {
            AbilityEventManager.Instance.TakeAbilityDamage -= TakeDamage;
        }
    }

    protected override void TakeDamage(BaseEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }
}
