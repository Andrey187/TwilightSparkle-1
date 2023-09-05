public class EnemySlime : BaseEnemy
{
    protected override void TakeDamage(IEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }
}
