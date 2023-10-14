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
        CalculateAndIncrementAngle(alternativeCount);
    }

    protected override void FixedRun()
    {
        if (_gamePause.IsPaused)
        {
            _thisRb.velocity = Vector3.zero;
            return;
        }

        Vector3 newPosition = _thisRb.position + _projectileMoveDirection * _speed * Time.fixedDeltaTime;
        _thisRb.MovePosition(newPosition);
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
}
