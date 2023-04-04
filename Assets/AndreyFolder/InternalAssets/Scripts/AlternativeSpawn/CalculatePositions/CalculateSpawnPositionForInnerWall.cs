using UnityEngine;

public class CalculateSpawnPositionForInnerWall : ParamsForCalculateSpawnPositions
{
    private Vector3 CalculatePosition<T>(T obj) where T : IObjectFactory
    {
        Vector3 pos = new Vector3(Random.Range(_centerGround.x - (_spawnAreaWidth / 2), _centerGround.x + (_spawnAreaWidth / 2)),
                                    _centerGround.y + _groundYSize / 2,
                                    Random.Range(_centerGround.z - (_spawnAreaHeight / 2), _centerGround.z + (_spawnAreaHeight / 2)));

        return pos;
    }

    public Vector3 RandomSetPositions<T>(T obj, int count) where T : IObjectFactory
    {
        Vector3 pos = CalculatePosition(obj);
        Transform[] objects = obj.CreateObjects(pos, count);
        foreach (Transform objTransform in objects)
        {
            if (objTransform != null)
            {
                _posPrefabs = new Vector3(pos.x, pos.y + objTransform.transform.localScale.y / 2, pos.z);
            }
        }
        return _posPrefabs;
    }

    public IRotationStrategy GetRandomRotation()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return new RotateMinus90();
            case 1:
                return new RotateZero();
            case 2:
                return new RotatePlus90();
            default:
                return new RotateZero();
        }
    }
}
