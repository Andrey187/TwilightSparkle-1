using UnityEngine;

public class ParamsForCalculateSpawnPositionsJob : MonoCache
{
    protected GameObject _ground;

    //For Bots
    protected Transform _player;
    protected PositionWritter _positionWritter;
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
        GameObject targetObject = GameObject.Find("PolyArtWizardStandardMat");
        _positionWritter = targetObject.transform.GetComponent<PositionWritter>();
        _player = _positionWritter.transform;
    }
}
