using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemySpawnMethodJob : ParamsForCalculateSpawnPositionsJob
{
    [SerializeField] protected float _spawnRadius = 1.5f;
    [SerializeField] protected float _colliderHitRadius = 0.3f;
    [SerializeField] protected float _groupSpawnCircleRadius = 20f;
    protected bool isGround = false;
    protected RaycastHit hit;
    private float distance;

    protected virtual void Start()
    {
        FindCameraBoundries();
    }

    protected void FindCameraBoundries()
    {
        _cameraHeight = CacheCamera.Instance._cameraHeight;
        _cameraWidth = CacheCamera.Instance._cameraWidth;

        float sqrX = _cameraWidth * _cameraWidth;
        float sqrZ = _cameraHeight * _cameraHeight;
        distance = Mathf.Sqrt(sqrX + sqrZ);
        _circleOutsideTheCameraField = distance / 2f;

    }
    
    protected internal Vector3 NewUnitCircleJob()
    {
        _randomInsideUnitCircle = new float3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y);
        _randomInsideUnitCircle.Normalize();
        _randomInsideUnitCircle *= _circleOutsideTheCameraField;
        return _randomInsideUnitCircle;
    }

    protected virtual internal void GroundCheck()
    {
        // Check the center of the spawn area
        Ray centerRay = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        bool hasHit = Physics.Raycast(centerRay, out hit, distanceToCheckGround);

        // Check the corners of the spawn area
        float angleStep = 360f / 16f;
        for (int i = 0; i < 16; i++)
        {
            Vector3 corner = _botsSpawnInRandomPointOnCircle + Quaternion.AngleAxis(i * angleStep, Vector3.up) * Vector3.forward * _groupSpawnCircleRadius;
            Ray cornerRay = new Ray(corner, Vector3.down);

            if (!Physics.Raycast(cornerRay, out hit, distanceToCheckGround))
            {
                hasHit = false;
                break; // no need to check other corners if one fails
            }
        }
        isGround = hasHit;
    }

    protected virtual internal void GroundCheck(Vector3 _botsSpawnInRandomPointOnCircle)
    {
        // Check the center of the spawn area
        Ray centerRay = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        bool hasHit = Physics.Raycast(centerRay, out hit, distanceToCheckGround);

        // Check the corners of the spawn area
        float angleStep = 360f / 16f;
        for (int i = 0; i < 16; i++)
        {
            Vector3 corner = _botsSpawnInRandomPointOnCircle + Quaternion.AngleAxis(i * angleStep, Vector3.up) * Vector3.forward * _groupSpawnCircleRadius;
            Ray cornerRay = new Ray(corner, Vector3.down);

            if (!Physics.Raycast(cornerRay, out hit, distanceToCheckGround))
            {
                hasHit = false;
                break; // no need to check other corners if one fails
            }
        }
        isGround = hasHit;
    }

    protected internal abstract bool ColliderCheck<T>(T bots, Vector3 pos) where T : Component;

    protected virtual void CheckSphere<T>(T obj, Vector3 vector) where T : Component
    {
        if (Physics.CheckSphere(vector, _colliderHitRadius))
        {
            if (obj != null)
            {
                if (isGround)
                {
                    Vector3 newPosition = obj.transform.position + vector; // Calculate new position relative to the current position
                    obj.transform.position = newPosition;

                    if (obj is BaseEnemy baseEnemy)
                    {
                        NavMeshAgent agent = baseEnemy.GetComponentInChildren<NavMeshAgent>();
                        agent.Warp(vector);
                    }
                }
            }
            else
            {
                Debug.LogWarning("GameObject is null or not on ground!");
            }
        }
    }

    protected internal abstract void SpawnEnemies();
}
