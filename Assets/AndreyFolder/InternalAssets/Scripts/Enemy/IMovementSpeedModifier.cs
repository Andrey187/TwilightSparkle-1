using UnityEngine.AI;

public interface IMovementSpeedModifier
{
    void ApplySpeedModifier(EnemyData enemyType, NavMeshAgent navMeshAgent, float amount, float duration);
}
