using UnityEngine;

public class WalkingState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("Run");
        Debug.Log("Run");
    }

    public override void Exit(IEnemy enemy)
    {
        
    }

    public override void Update(IEnemy enemy)
    {
        // Move towards the target position
    }
}
