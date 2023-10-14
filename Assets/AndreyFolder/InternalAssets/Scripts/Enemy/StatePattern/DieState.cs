public class DieState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("Die");
    }

    public override void Exit(IEnemy enemy)
    {
        enemy.MeshAnimator.Pause();
    }

    public override void Update(IEnemy enemy)
    {
        // Handle any logic related to taking damage
    }
}
