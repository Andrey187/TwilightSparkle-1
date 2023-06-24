using UnityEngine;

public class TurtleMoveable : BaseBotsMoveable
{
    protected override void Awake()
    {
        base.Awake();
        GameObject targetObject = GameObject.Find("PolyArtWizardStandardMat");
        PositionWritter positionWritter = targetObject.transform.GetComponent<PositionWritter>();
        _targetPosition = positionWritter._position;
    }

    protected override void Run()
    {
        base.Run();
    }
}
