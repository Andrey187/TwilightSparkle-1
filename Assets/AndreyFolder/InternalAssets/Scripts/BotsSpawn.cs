using UnityEngine;
using System.Collections;

public class BotsSpawn : MonoCache
{
    [SerializeField] private SpawnAreaCalculation _spawnAreaCalculation;
    [SerializeField] private Transform _player;
    //[SerializeField] private float _spawnTime = 5f;

    public IEnumerator SpawnEnemies(StageEvent stageEvent)
    {
        yield return new WaitForSeconds(0.1f);
        _spawnAreaCalculation.NewUnitCircle();
        _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField();
        _spawnAreaCalculation.GroundCheck();

        if (stageEvent.EnemyCount <
            stageEvent.SpawnCount)
        {
            yield return new WaitForSeconds(0.7f);
            Spawn(stageEvent);
            
            //Debug.Log(stageEvent.Type + " spawnName");
        }
        
    }


    /// <summary>
    /// Метод спавнящий группу объектов в области случайной точки на окружности
    /// </summary>

    private void Spawn(StageEvent type)
    {
        _spawnAreaCalculation.SpawnGroupInFieldOnCircle();
        GameObject bots = ObjectPooler.Instance.GetObject(type.Type);
        _spawnAreaCalculation.ColliderCheck(bots);
        type.EnemyCount++;
        //Debug.Log(type.Prefab + "  botsInit  " + type.Type);
    }




    //public IEnumerator SpawnEnemies(StageData stageData, StageEvent.ObjectType type)
    //{
    //    yield return new WaitForSeconds(_spawnTime);
    //    _spawnAreaCalculation.NewUnitCircle();
    //    _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField();
    //    _spawnAreaCalculation.GroundCheck();

    //    if (stageEvent.EnemyCount < stageEvent.SpawnCount)
    //    {
    //        yield return new WaitForSeconds(0.7f);
    //        Spawn(type);
    //        stageEvent.EnemyCount++;
    //    }

    //    StartCoroutine(SpawnEnemies(stageData, stageEvent, type));
    //}





    //public IEnumerator SpawnEnemies(StageEvent stageEvent, GameObject go)
    //{
    //    yield return new WaitForSeconds(_spawnTime);
    //    _spawnAreaCalculation.SpawnOnCircleOutsideTheCameraField();
    //    _spawnAreaCalculation.NewUnitCircle();
    //    _spawnAreaCalculation.GroundCheck();
    //    yield return new WaitForSeconds(0.7f);
    //    Spawn(stageEvent, go);
    //    StartCoroutine(SpawnEnemies(stageEvent, go));
    //}
}
