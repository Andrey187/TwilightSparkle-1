using UnityEngine;
using System.Collections;

public class BotsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemiesPrefabs;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTime = 1.5f;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _circleRadius = 1f;
    private Vector2 spawnPos;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        spawnPos = _player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * _spawnRadius;

        Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Length)], new Vector3( spawnPos.x, 0f, spawnPos.y), Quaternion.identity);

        yield return new WaitForSeconds(_spawnTime);
        StartCoroutine(SpawnEnemies());
    }
}
