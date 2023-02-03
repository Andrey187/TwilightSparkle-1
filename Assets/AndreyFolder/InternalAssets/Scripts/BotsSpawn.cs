using UnityEngine;
using System.Collections;

public class BotsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemiesPrefabs;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTime = 1.5f;
    [SerializeField] private GameObject _player;
    private Vector2 spawnPos;

    private Camera _cam;
    private float _cameraWidth;
    private float _cameraHeight;
    
    private float _circleRadius;
    private Vector2 randomPointOnCircle;

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
        FindBoundries();
        RandomOnUnitCircle(_circleRadius);
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_spawnTime);

        spawnPos = _player.transform.position;
        spawnPos += randomPointOnCircle * _spawnRadius;
        Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Length)], new Vector3(spawnPos.x, 0f, spawnPos.y), Quaternion.identity);

        StartCoroutine(SpawnEnemies());
    }

    private void FindBoundries()
    {
        _cameraWidth = 1 / (_cam.WorldToViewportPoint(new Vector3(1, 0, 1)).x - .5f);
        _cameraHeight = 1 / (_cam.WorldToViewportPoint(new Vector3(1, 0, 1)).y - .5f);

        //Теорема Пифагора епта
        float sqrX = _cameraWidth * _cameraWidth;
        float sqrZ = _cameraHeight * _cameraHeight;
        float distance = Mathf.Sqrt(sqrX + sqrZ);
        _circleRadius = distance / 2;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_player.transform.position, new Vector3(_player.transform.position.x, 0f, _circleRadius));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_player.transform.position, _circleRadius);
    }

    private Vector2 RandomOnUnitCircle(float radius)
    {
        randomPointOnCircle = Random.insideUnitCircle;
        randomPointOnCircle.Normalize();
        randomPointOnCircle *= radius;
        return randomPointOnCircle;
    }
}
