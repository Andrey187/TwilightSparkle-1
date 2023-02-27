using UnityEngine;

public class SpawnAreaCalculation : MonoCache
{
    [SerializeField] private float _spawnRadius = 1.5f;
    [SerializeField] private float _colliderHitRadius = 0.3f;
    [SerializeField] private float _groupSpawnСircleRadius = 20f;

    private Camera _cam;
    private Transform _player;
    private float _cameraWidth;
    private float _cameraHeight;

    private float _circleOutsideTheCameraField;

    private Vector3 _spawnCircleRadius;
    private Vector3 _randomInsideUnitCircle;
    private Vector3 _botsSpawnInRandomPointOnCircle;
    private Vector3 _botsSpawnField;

    private float distanceToCheckGround = 2f;
    private bool isGround = false;
    private RaycastHit hit;

    private void Awake()
    {
        _cam = Camera.main;
        _player = Find<PositionWritter>().transform;
        FindCameraBoundries();
    }

    /// <summary>
    /// Метод спавнящий объекты в случайной точке на окружности вне видимости камеры
    /// </summary>
    public void SpawnOnCircleOutsideTheCameraField()
    {
        _spawnCircleRadius = _player.transform.position + _randomInsideUnitCircle * _spawnRadius;
        _botsSpawnInRandomPointOnCircle = new Vector3(_spawnCircleRadius.x,
            0f,
            _player.transform.position.z + _spawnCircleRadius.y);
    }

    public Vector2 NewUnitCircle()
    {
        _randomInsideUnitCircle = Random.insideUnitCircle;
        _randomInsideUnitCircle.Normalize();
        _randomInsideUnitCircle *= _circleOutsideTheCameraField;
        return _randomInsideUnitCircle;
    }

    public void SpawnGroupInFieldOnCircle()
    {
        _botsSpawnField = _botsSpawnInRandomPointOnCircle +
            new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y)
            * _groupSpawnСircleRadius;
    }

    //ToDo возможно в дальнейшем можно будет изменить условие проверки,
    //типо по сфере чекать попадание стен в радиус
    //решить проблему с навмешем. 
    public void GroundCheck()
    {
        Ray ray = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        isGround = Physics.Raycast(ray, out hit, distanceToCheckGround);
    }

    public delegate void Del();
    
    public void ColliderCheck(GameObject bots, Del del)
    {
        
        if (!Physics.CheckSphere(_botsSpawnField, _colliderHitRadius) && isGround)
        {
            del?.Invoke();
            if (bots == null)
            {
                Debug.LogWarning("GameObject is null!");
                return;
            }
            bots.GetComponent<BotsLifeTime>().OnCreate(_botsSpawnField, Quaternion.identity);
            bots.GetComponentInChildren<Rigidbody>().transform.position =
                bots.GetComponent<BotsLifeTime>().transform.position;
            
        }
    }

    /// <summary>
    /// Метод рассчета границ камеры и окружности
    /// </summary>
    public void FindCameraBoundries()
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
        if (_player !=null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_player.transform.position, _circleOutsideTheCameraField);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_botsSpawnInRandomPointOnCircle, _groupSpawnСircleRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(_botsSpawnInRandomPointOnCircle, Vector3.down);
        }
    }
}
