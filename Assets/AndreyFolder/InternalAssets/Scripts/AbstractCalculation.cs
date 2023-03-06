using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCalculation
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

    private float distanceToCheckGround = 2f;
    private bool isGround = false;
    private RaycastHit hit;

    private float _spawnRadius;
    private float _colliderHitRadius;
    private float _groupSpawn�ircleRadius;

    public AbstractCalculation(float spawnRadius, float colliderHitRadius, float groupSpawn�ircleRadius, Transform player)
    {
        _spawnRadius = spawnRadius;
        _colliderHitRadius = colliderHitRadius;
        _groupSpawn�ircleRadius = groupSpawn�ircleRadius;
        _player = player;
    }

    private void Awake()
    {
        _cam = Camera.main;
        FindCameraBoundries();
    }

    /// <summary>
    /// ����� ��������� ������� � ��������� ����� �� ���������� ��� ��������� ������
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
            * _groupSpawn�ircleRadius;
    }

    //ToDo �������� � ���������� ����� ����� �������� ������� ��������,
    //���� �� ����� ������ ��������� ���� � ������
    //������ �������� � ��������. 
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
            //bots.GetComponent<BotsLifeTime>().OnCreate(_botsSpawnField, Quaternion.identity);
            bots.GetComponentInChildren<Rigidbody>().transform.position =
                bots.GetComponent<BotsLifeTime>().transform.position;

        }
    }

    /// <summary>
    /// ����� �������� ������ ������ � ����������
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
        if (_player != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_player.transform.position, _circleOutsideTheCameraField);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_botsSpawnInRandomPointOnCircle, _groupSpawn�ircleRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(_botsSpawnInRandomPointOnCircle, Vector3.down);
        }
    }
}
