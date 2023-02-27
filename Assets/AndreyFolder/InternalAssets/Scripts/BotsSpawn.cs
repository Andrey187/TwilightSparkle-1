using UnityEngine;
using System.Collections;

public class BotsSpawn : MonoBehaviour
{
    [SerializeField] private SpawnAreaCalculation _spawnAreaCalculation;
    [SerializeField] private Transform _player;
    [SerializeField] private float _spawnTime = 5f;
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
        _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField();
        _spawnAreaCalculation.NewUnitCircle();
        _spawnAreaCalculation.GroundCheck();
        yield return new WaitForSeconds(0.7f);
        Spawn();
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Метод спавнящий группу объектов в области случайной точки на окружности
    /// </summary>

    private void Spawn()
    {
        _spawnAreaCalculation.SpawnGroupInFieldOnCircle();
        _spawnAreaCalculation.ColliderCheck(bots, BotsInit);
    }
}
