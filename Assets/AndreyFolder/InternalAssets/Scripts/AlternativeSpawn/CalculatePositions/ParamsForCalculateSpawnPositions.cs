using UnityEngine;
using Zenject;

public class ParamsForCalculateSpawnPositions : MonoCache
{
    protected GameObject _ground;
    
    //For Bots
    protected Transform _player;
    [Inject]protected PositionWritter _positionWritter;
    protected float _cameraWidth;
    protected float _cameraHeight;
    protected float distance;
    protected float _circleOutsideTheCameraField;

    protected Vector3 _spawnCircleRadius;
    protected Vector3 _randomInsideUnitCircle;
    protected Vector3 _botsSpawnInRandomPointOnCircle;
    protected Vector3 _botsSpawnField;

    protected const float distanceToCheckGround = 3f;


    private void Awake()
    {
        _ground = GameObject.FindGameObjectWithTag("Plane");

        _player = _positionWritter.transform;
    }

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
}
