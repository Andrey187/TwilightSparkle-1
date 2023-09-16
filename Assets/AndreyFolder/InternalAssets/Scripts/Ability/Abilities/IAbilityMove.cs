using UnityEngine;

public interface IAbilityMove
{
    void MoveWithPhysics(Vector3 endPoint, Vector3 startPoint);

    void MoveWithPhysics();
}
