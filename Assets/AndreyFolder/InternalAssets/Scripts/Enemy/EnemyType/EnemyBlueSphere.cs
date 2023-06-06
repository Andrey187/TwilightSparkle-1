public class EnemyBlueSphere : BaseEnemy
{
    protected override void OnEnabled()
    {
        base.OnEnabled();
        EventManager.Instance.TakeAbilityDamage += TakeDamage;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        if (EventManager.Instance != null)
        {
            EventManager.Instance.TakeAbilityDamage -= TakeDamage;
        }
    }

    protected override void TakeDamage(BaseEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }
}
