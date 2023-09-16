public class EnemyBeholder : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
    }

    protected override void TakeDamage(IEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }
}
