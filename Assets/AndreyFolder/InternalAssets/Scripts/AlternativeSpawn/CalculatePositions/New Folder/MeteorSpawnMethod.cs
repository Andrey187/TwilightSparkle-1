using UnityEngine;
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
