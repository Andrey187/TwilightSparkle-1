using UnityEngine;

public class ChestBoss : BaseEnemy
{
    protected override void TakeDamage(IEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }

    protected internal override void Die()
    {
        base.Die();

    }
}
