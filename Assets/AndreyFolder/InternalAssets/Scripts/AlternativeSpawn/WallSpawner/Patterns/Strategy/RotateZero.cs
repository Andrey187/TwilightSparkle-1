using UnityEngine;

public class RotateZero : IRotationStrategy
{
    public Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
