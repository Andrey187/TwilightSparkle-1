using UnityEngine;

public class BeholderSpawnMethodJob : EnemySpawnMethodJob
{
    private Transform _startPointBeholderSpawn;

    protected override void Start()
    {
        base.Start();
        GameObject targetObject = GameObject.Find("StartPointEnemySpawn");
        _startPointBeholderSpawn = targetObject.transform;
    }

    protected internal override void SpawnEnemies()
    {
        // Implement the logic for spawning enemies in a circle
        Vector2 randomPoint = Random.insideUnitCircle * _spawnRadius;

        _botsSpawnInRandomPointOnCircle = _startPointBeholderSpawn.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    protected internal override bool ColliderCheck<T>(T bots, Vector3 pos)
    {
        throw new System.NotImplementedException();
    }
}
