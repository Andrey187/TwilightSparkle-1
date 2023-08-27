using UnityEngine;
using Zenject;

public class MultipleFireBallMovement : BaseAbilityMovement, IMultipleProjectileCalculatePosition
{
    [Inject] private MultipleFireBallAbility _multipleFireBallAbility;
    private Vector3 _projectileMoveDirection;
    private static float _currentAngle = 0f;

    protected override void OnEnabled()
    {
        int alternativeCount = _multipleFireBallAbility.AlternativeCountAbilities;
        CalculateAlternativeMovePosition();
        MoveWithPhysics();
        CalculateAndIncrementAngle(alternativeCount);
    }

    public void CalculateAndIncrementAngle(float countOfProjectiles)
    {
        float angleStep = 360f / countOfProjectiles;
        _currentAngle += angleStep;
    }

    public void CalculateAlternativeMovePosition()
    {
        float dirX = Mathf.Sin(_currentAngle * Mathf.Deg2Rad);
        float dirZ = Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        _projectileMoveDirection = new Vector3(dirX, 0, dirZ);
    }

    public override void MoveWithPhysics()
    {
        _thisRb.velocity = _projectileMoveDirection * _speed;
    }
}
