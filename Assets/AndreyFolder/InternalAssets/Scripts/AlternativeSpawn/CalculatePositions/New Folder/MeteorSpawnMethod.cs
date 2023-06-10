using UnityEngine;
using UnityEngine.AI;

public class MeteorSpawnMethod : GroupSpawnMethod
{
    protected override void CheckSphere<T>(T obj, Vector3 vector)
    {
        if (obj != null)
        {
            if (isGround)
            {
                Vector3 newPosition = obj.transform.position + vector; // Calculate new position relative to the current position
                obj.transform.position = newPosition;
                
                if (obj is BaseEnemy baseEnemy)
                {
                    NavMeshAgent agent = baseEnemy.GetComponent<NavMeshAgent>();
                    agent.Warp(vector);
                }
            }
        }
    }

    protected internal override void SpawnEnemies()
    {
        base.SpawnEnemies();
    }

    protected internal override bool ColliderCheck<T>(T obj)
    {
        CheckSphere(obj, new Vector3(_botsSpawnField.x, 10f, _botsSpawnField.z));
        return isGround;
    }
}
