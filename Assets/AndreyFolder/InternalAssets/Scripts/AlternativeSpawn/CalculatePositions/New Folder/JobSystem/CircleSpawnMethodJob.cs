using UnityEngine;

public class CircleSpawnMethodJob : EnemySpawnMethodJob
{
    protected internal override void SpawnEnemies()
    {
        // Implement the logic for spawning enemies in a circle

        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x,
            0,
            _player.transform.position.z + _spawnCircleRadius.y);

    }

    protected internal override bool ColliderCheck<T>(T bots, Vector3 pos)
    {
        CheckSphere(bots, pos);
        return isGround;
    }
}
