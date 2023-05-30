using UnityEngine;

public class GroupSpawnMethod : EnemySpawnMethod
{
    protected internal override void SpawnEnemies(WaveSpawner.Wave wave)
    {
        // Implement the logic for spawning enemies in a circle

        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x,
            0,
            _player.transform.position.z + _spawnCircleRadius.y);
        
        Vector2 randomInsideUnitCircle2D = Random.insideUnitCircle;
        _botsSpawnField = _botsSpawnInRandomPointOnCircle + _ground.transform.position +
            new Vector3(randomInsideUnitCircle2D.x, 0f, randomInsideUnitCircle2D.y)
            * _groupSpawn—ircleRadius;
    }

    protected internal override bool ColliderCheck(GameObject bots)
    {
        CheckSphere(bots, _botsSpawnField);
        return isGround;
    }
}
