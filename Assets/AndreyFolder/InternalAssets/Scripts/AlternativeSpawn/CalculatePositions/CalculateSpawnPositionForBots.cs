using UnityEngine;
using UnityEngine.AI;

public class CalculateSpawnPositionForBots : ParamsForCalculateSpawnPositions
{
    [SerializeField] private float _spawnRadius = 1.5f;
    [SerializeField] private float _colliderHitRadius = 0.3f;
    [SerializeField] private float _groupSpawn—ircleRadius = 20f;
    private bool isGround = false;
    private RaycastHit hit;

    public Vector2 NewUnitCircle()
    {
        _randomInsideUnitCircle = Random.insideUnitCircle;
        _randomInsideUnitCircle.Normalize();
        _randomInsideUnitCircle *= _circleOutsideTheCameraField;
        return _randomInsideUnitCircle;
    }

    public void SpawnOnCircleOutsideTheCameraField()
    {
        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x,
            0,
            _player.transform.position.z + _spawnCircleRadius.y);
    }

    public void SpawnGroupInFieldOnCircle()
    {
        Vector2 randomInsideUnitCircle2D = Random.insideUnitCircle;
        _botsSpawnField = _botsSpawnInRandomPointOnCircle + _ground.transform.position +
            new Vector3(randomInsideUnitCircle2D.x, 0f, randomInsideUnitCircle2D.y)
            * _groupSpawn—ircleRadius;
    }

    public void GroundCheck()
    {
        // Check the center of the spawn area
        Ray centerRay = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        bool hasHit = Physics.Raycast(centerRay, out hit, distanceToCheckGround);

        // Check the corners of the spawn area
        float angleStep = 360f / 16f;
        for (int i = 0; i < 16; i++)
        {
            Vector3 corner = _botsSpawnInRandomPointOnCircle + Quaternion.AngleAxis(i * angleStep, Vector3.up) * Vector3.forward * _groupSpawn—ircleRadius;
            Ray cornerRay = new Ray(corner, Vector3.down);
            if (!Physics.Raycast(cornerRay, out hit, distanceToCheckGround))
            {
                hasHit = false;
                break; // no need to check other corners if one fails
            }
        }
        isGround = hasHit;
    }

    public bool ColliderCheck(GameObject bots)
    {
        SpawnGroupInFieldOnCircle();
        NavMeshAgent agent = bots.GetComponent<NavMeshAgent>();
        BaseEnemy objLifeTime = bots.GetComponent<BaseEnemy>();

        if (Physics.CheckSphere(_botsSpawnField, _colliderHitRadius))
        {
            if (bots != null)
            {
                if (isGround)
                {
                    objLifeTime?.OnCreate(_botsSpawnField, Quaternion.identity);
                    agent.Warp(_botsSpawnField);
                }
            }
            else
            {
                Debug.LogWarning("GameObject is null or not on ground!");
            }
        }
        return isGround;
    }

    private void OnDrawGizmosSelected()
    {
        if (_player != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_player.transform.position, _circleOutsideTheCameraField);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_botsSpawnInRandomPointOnCircle, _groupSpawn—ircleRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(_botsSpawnInRandomPointOnCircle, Vector3.down);
        }
    }
}
