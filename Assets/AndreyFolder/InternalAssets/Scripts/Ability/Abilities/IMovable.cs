using UnityEngine;

public interface IMovable
{
    void MoveWithPhysics(Transform endPoint, Transform startPoint);

    void MoveWithPhysics();
}
