using UnityEngine;

public class BeholderMoveable : BaseBotsMoveable
{
    protected override void Awake()
    {
        base.Awake();
        GameObject targetObject = GameObject.Find("EndPointEnemySpawn");
        PositionWritter positionWritter = targetObject.transform.GetComponent<PositionWritter>();
        _targetPosition = positionWritter._position;
    }

    protected override void Run()
    {
        base.Run();
    }
}
