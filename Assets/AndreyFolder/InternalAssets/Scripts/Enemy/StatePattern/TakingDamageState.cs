public class TakingDamageState : EnemyState
{
    private const string AnimationTrigger = "TakeDamage";

    public override void Enter(BaseEnemy enemy)
    {
        //enemy.Animator.SetTrigger(AnimationTrigger);
        enemy.meshAnimator.Play("GetHit");
    }

    public override void Exit(BaseEnemy enemy)
    {
        // Return to previous state or transition to a different state
    }

    public override void Update(BaseEnemy enemy)
    {
        // Handle any logic related to taking damage
    }
}
