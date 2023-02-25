using UnityEngine;
using System.Collections;

public class BotsSpawn : MonoBehaviour
{
    [SerializeField] private SpawnAreaCalculation _spawnAreaCalculation;
    [SerializeField] private Transform _player;
    [SerializeField] private float _spawnRadius = 1f;
    [SerializeField] private float _spawnTime = 1.5f;
    [SerializeField] private float _colliderHitRadius = 2f;
    [SerializeField] private float _groupSpawnСircleRadius = 5f;
    private static GameObject bots;

    
    //ToDo
    private StageEvent _stageEvent;
    private void BotsInit()
    {
        bots = ObjectPooler.Instance.GetObject(_stageEvent.Type);
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_spawnTime);
        _spawnAreaCalculation.GroundCheck();
        _spawnAreaCalculation.NewUnitCircle();
        _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField(_spawnRadius);
        yield return new WaitForSeconds(0.5f);
        Spawn();
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Метод спавнящий группу объектов в области случайной точки на окружности
    /// </summary>

    private void Spawn()
    {
        _spawnAreaCalculation.SpawnGroupInFieldOnCircle(_groupSpawnСircleRadius);
        BotsInit();
        if (!Physics.CheckSphere(_spawnAreaCalculation.BotsSpawnField, _colliderHitRadius) && _spawnAreaCalculation.IsGround)
        {
            bots.GetComponent<BotsLifeTime>().OnCreate(_spawnAreaCalculation.BotsSpawnField, Quaternion.identity);
            bots.GetComponentInChildren<Rigidbody>().transform.position =
                bots.GetComponent<BotsLifeTime>().transform.position;
        }

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawCube(_player.transform.position, new Vector3(_cameraWidth, 0f, _cameraHeight));
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(_player.transform.position, _circleOutsideTheCameraField);

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(_botsSpawnInRandomPointOnCircle, _groupSpawnСircleRadius);

    //    Gizmos.color = Color.black;
    //    Gizmos.DrawRay(_botsSpawnInRandomPointOnCircle, Vector3.down);

    //    if (bots != null)
    //    {
    //        Gizmos.color = Color.black;
    //        Gizmos.DrawRay(bots.transform.position, Vector3.down);
    //    }
    //}
}
