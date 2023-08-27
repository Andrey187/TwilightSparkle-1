using UnityEngine;

public class BeholderSpawnMethod : SpawnMethod
{
    private Transform _startPointBeholderSpawn;

    protected override void Start()
    {
        base.Start();
        GameObject targetObject = GameObject.Find("StartPointEnemySpawn");
        _startPointBeholderSpawn = targetObject.transform;
    }

    protected internal override void SpawnPrefabs()
    {
        // Implement the logic for spawning enemies in a circle
        Vector2 randomPoint = Random.insideUnitCircle * _spawnRadius;

        _botsSpawnInRandomPointOnCircle = _startPointBeholderSpawn.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    protected internal override bool ColliderCheck<T>(T bots)
    {
        CheckSphere(bots, _botsSpawnInRandomPointOnCircle);
        return isGround;
    }
}
