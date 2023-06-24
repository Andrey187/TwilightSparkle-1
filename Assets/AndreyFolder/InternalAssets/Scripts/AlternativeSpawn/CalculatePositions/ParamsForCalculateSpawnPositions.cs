using UnityEngine;

public class ParamsForCalculateSpawnPositions : MonoCache
{
    protected GameObject _ground;
    //For Wall
    protected BoxCollider _colliderOuterWall, _colliderGround;
    protected float xSizeOuterWall, ySizeOuterWall, zSizeOuterWall;
    protected float xSizeGround, ySizeGround, zSizeGround;
    protected Vector3 xScaleOuterWall, centerOuterWall;
    protected Vector3 _centerGround;

    protected Vector3 _posPrefabs;
    protected float _groundWidth, _groundHeight, _groundXSize, _groundYSize, _spawnAreaWidth, _spawnAreaHeight;

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

    protected const float distanceToCheckGround = 2f;


    private void Awake()
    {
        _ground = GameObject.FindGameObjectWithTag("Plane");
        GameObject targetObject = GameObject.Find("PolyArtWizardStandardMat");
        _positionWritter = targetObject.transform.GetComponent<PositionWritter>();
        _player = _positionWritter.transform;
    }

    void Start()
    {
        ParamsForInnerWalls();
    }

    public void ParamsForWall(Transform _listWall)
    {
        //Params Collider
        xSizeOuterWall = _colliderOuterWall.size.x;
        ySizeOuterWall = _colliderOuterWall.size.y;
        zSizeOuterWall = _colliderOuterWall.size.z;
        centerOuterWall = _colliderOuterWall.center;

        //Params Scale
        xScaleOuterWall = _listWall.transform.localScale;
    }

    public void ParamsForGround()
    {
        //Params Collider
        xSizeGround = _colliderGround.size.x;
        ySizeGround = _colliderGround.size.y;
        zSizeGround = _colliderGround.size.z;
        _centerGround = _colliderGround.center;

        //Params Scale
        _groundXSize = _ground.transform.localScale.x;
        _groundYSize = _ground.transform.localScale.y;
    }

    private void ParamsForInnerWalls()
    {
        _groundWidth = _ground.GetComponent<Renderer>().bounds.size.x; // Get the width of the plane object
        _groundHeight = _ground.GetComponent<Renderer>().bounds.size.z; // Get the height of the plane object
        _centerGround = _ground.transform.position; // Get the center position of the plane object
        
        _spawnAreaWidth = _groundWidth * 0.7f; // Calculate the width of the area to spawn objects in (30% less than the width of the plane)
        _spawnAreaHeight = _groundHeight * 0.7f; // Calculate the height of the area to spawn objects in (30% less than the height of the plane)
    }
}
