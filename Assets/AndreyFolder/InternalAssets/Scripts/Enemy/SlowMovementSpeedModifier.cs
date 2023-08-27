using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SlowMovementSpeedModifier : IMovementSpeedModifier
{
    public async void ApplySpeedModifier(EnemyData enemyType, NavMeshAgent navMeshAgent, float amount, float duration)
    {
        // Calculate the reduced speed
        float currentSpeed = navMeshAgent.speed * amount;

        // Ensure the new speed is above the threshold (e.g., 1f) and not lower than half of the initial speed
        currentSpeed = Mathf.Max(currentSpeed, 1f);

        // Apply the reduced speed
        navMeshAgent.speed = currentSpeed;

        await Task.Delay(Mathf.FloorToInt(duration * 1000)); // Convert seconds to milliseconds

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = enemyType.Speed;
        }
    }
}
