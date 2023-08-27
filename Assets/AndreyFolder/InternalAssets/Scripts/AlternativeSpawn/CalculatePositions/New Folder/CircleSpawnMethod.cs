using UnityEngine;

public class CircleSpawnMethod : SpawnMethod
{
    protected internal override void SpawnPrefabs()
    {
        // Implement the logic for spawning enemies in a circle

        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x,
            0,
            _player.transform.position.z + _spawnCircleRadius.y);
        
    }

    protected internal override bool ColliderCheck<T>(T bots)
    {
        CheckSphere(bots, _botsSpawnInRandomPointOnCircle);
        return isGround;
    }
}
