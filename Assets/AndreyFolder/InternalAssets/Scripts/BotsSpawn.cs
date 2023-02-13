using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotsSpawn : MonoBehaviour
{
    [SerializeField] private ObjectPooler.ObjectInfo.ObjectType _botsType;
    [SerializeField] private GameObject _enemiesPrefabs;
    [SerializeField] private Transform _player;
    [SerializeField] private float _spawnRadius = 1f;
    [SerializeField] private float _spawnTime = 1.5f;
    [SerializeField] private float _colliderHitRadius = 2f;
    [SerializeField] private float _spawnCount = 20f;
    [SerializeField] private float _groupSpawnСircleRadius = 5f;

    private Camera _cam;
    private float _cameraWidth;
    private float _cameraHeight;

    private float _circleOutsideTheCameraField;

    private GameObject bots;
    private Vector3 _spawnCircleRadius;
    private Vector3 _randomInsideUnitCircle;
    private Vector3 _botsSpawnInRandomPointOnCircle;
    private Vector3 _botsSpawnField;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        FindCameraBoundries();
        NewUnitCircle(out _randomInsideUnitCircle, _circleOutsideTheCameraField); //random points на окружности вне видимости камеры
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void BotsInit()
    {
        bots = ObjectPooler.Instance.GetObject(_botsType);
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_spawnTime);
        SpawnOnCircleOutsideTheCameraField();
        yield return new WaitForSeconds(0.5f);
        SpawnGroupInFieldonCircle();
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Метод спавнящий объекты в случайной точке на окружности вне видимости камеры
    /// </summary>
    private void SpawnOnCircleOutsideTheCameraField()
    {
        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x, 
            0f, 
            _player.transform.position.z + _spawnCircleRadius.y);
    }

    private Vector2 NewUnitCircle(out Vector3 newCircle, float radius)
    {
        newCircle = Random.insideUnitCircle;
        newCircle.Normalize();
        newCircle *= radius;
        return newCircle;
    }

    /// <summary>
    /// Метод спавнящий группу объектов в области случайной точки на окружности
    /// </summary>
    private void SpawnGroupInFieldonCircle()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            _botsSpawnField = _botsSpawnInRandomPointOnCircle +
                new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y)
                * _groupSpawnСircleRadius;

            if (!Physics.CheckSphere(_botsSpawnField, _colliderHitRadius) && isGround)
            {
                BotsInit();
                bots.GetComponent<BotsLifeTime>().OnCreate(_botsSpawnField, Quaternion.identity);
               
            }
        }
    }

    public float distanceToCheck = 1f;
    public bool isGround = false;
    public RaycastHit hit;

    //ToDo возможно в дальнейшем можно будет изменить условие проверки,
    //типо по сфере чекать попадание стен в радиус
    private void GroundCheck()
    {
        Ray ray = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        isGround = Physics.Raycast(ray, out hit, distanceToCheck);
    }

    /// <summary>
    /// Метод рассчета границ камеры и окружности
    /// </summary>
    private void FindCameraBoundries()
    {
        _cameraWidth = 1 / (_cam.WorldToViewportPoint(new Vector3(1, 0, 1)).x - .5f);
        _cameraHeight = 1 / (_cam.WorldToViewportPoint(new Vector3(1, 0, 1)).y - .5f);

        float sqrX = _cameraWidth * _cameraWidth;
        float sqrZ = _cameraHeight * _cameraHeight;
        float distance = Mathf.Sqrt(sqrX + sqrZ);
        _circleOutsideTheCameraField = distance / 2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_player.transform.position, _circleOutsideTheCameraField);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_botsSpawnInRandomPointOnCircle, _groupSpawnСircleRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(_botsSpawnInRandomPointOnCircle, Vector3.down);

        if(bots != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(bots.transform.position, Vector3.down);
        }
    }
}
