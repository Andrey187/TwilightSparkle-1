using UnityEngine;

public class BossTurtle : BaseEnemy
{
    protected override void TakeDamage(IEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
    }
}
