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
}
