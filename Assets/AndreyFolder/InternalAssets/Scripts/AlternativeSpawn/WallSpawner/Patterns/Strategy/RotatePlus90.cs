using UnityEngine;

public class RotatePlus90 : IRotationStrategy
{
    public Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(new Vector3(0f, -90f, 0f));
    }
}
