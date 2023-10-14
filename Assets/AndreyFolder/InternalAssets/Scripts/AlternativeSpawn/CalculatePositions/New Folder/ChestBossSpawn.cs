using UnityEngine;

public class ChestBossSpawn : SpawnMethod
{
    [SerializeField] private Transform _startPointBeholderSpawn;

    protected override void Start()
    {
        base.Start();
    }

    protected internal override void SpawnPrefabs() { }

    protected internal override bool ColliderCheck<T>(T bots)
    {
        CheckSphere(bots, _startPointBeholderSpawn.transform.position);
        return isGround;
    }
}
