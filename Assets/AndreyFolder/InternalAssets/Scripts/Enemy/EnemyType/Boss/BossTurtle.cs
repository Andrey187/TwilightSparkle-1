using UnityEngine;

public class BossTurtle : BaseEnemy
{
    protected override void OnEnabled()
    {
        base.OnEnabled();
        AbilityEventManager.Instance.TakeAbilityDamage += TakeDamage;
    }

    protected override void OnDisabled()
    {
        base.OnEnabled();
        if (AbilityEventManager.Instance != null)
        {
            AbilityEventManager.Instance.TakeAbilityDamage -= TakeDamage;
        }
    }

    protected override void Run()
    {
    }

    protected override void TakeDamage(BaseEnemy enemy, int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        base.TakeDamage(enemy, damageAmount, ability, doTEffect);
        Debug.Log("damage");
    }
}
