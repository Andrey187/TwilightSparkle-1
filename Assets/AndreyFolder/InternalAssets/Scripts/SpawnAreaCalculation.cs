using UnityEngine;

public class SpawnAreaCalculation : MonoCache
{
    private Camera _cam;
    private Transform _player;
    private float _cameraWidth;
    private float _cameraHeight;

    private float _circleOutsideTheCameraField;

    private Vector3 _spawnCircleRadius;
    private Vector3 _randomInsideUnitCircle;
    private Vector3 _botsSpawnInRandomPointOnCircle;
    private Vector3 _botsSpawnField;
    public Vector3 BotsSpawnField { get { return _botsSpawnField; } set { _botsSpawnField = value; } }


    private float distanceToCheckGround = 1f;
    private bool isGround = false;
    public bool IsGround { get { return isGround; } set { isGround = value; } }
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
    public void SpawnOnCircleOutsideTheCameraField(float _spawnRadius)
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

    public void SpawnGroupInFieldOnCircle(float _groupSpawnRadius)
    {
        _botsSpawnField = _botsSpawnInRandomPointOnCircle +
            new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y)
            * _groupSpawnRadius;
    }

    //ToDo возможно в дальнейшем можно будет изменить условие проверки,
    //типо по сфере чекать попадание стен в радиус
    public void GroundCheck()
    {
        Ray ray = new Ray(_botsSpawnInRandomPointOnCircle, Vector3.down);
        isGround = Physics.Raycast(ray, out hit, distanceToCheckGround);
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
}
