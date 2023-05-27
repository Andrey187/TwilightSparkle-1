using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private enum MovementState
    {
        Idle,
        Running
    }

    private CharacterMotor characterMotor;
    private CharacterAnimator characterAnimator;
    private MovementState currentState;

    private void Awake()
    {
        characterMotor = GetComponent<CharacterMotor>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    private void Start()
    {
        SetState(MovementState.Idle);
    }

    public void HandleInput(float horizontal, float vertical)
    {
        Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;
        MovementState nextState = GetCurrentState(movementDirection);
        if (nextState != currentState)
            SetState(nextState);

        int movementSpeed = GetCurrentMovementSpeed();
        characterMotor.Move(movementDirection, movementSpeed);
    }

    public void HandleAim(float horizontal, float vertical)
    {
        Vector3 AimDirection = new Vector3(horizontal, 0f, vertical).normalized;
        characterMotor.Rotate(AimDirection);
    }

    private MovementState GetCurrentState(Vector3 movementDirection)
    {
        if (movementDirection == Vector3.zero)
            return MovementState.Idle;
        else 
            return MovementState.Running;
    }

    private void SetState(MovementState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case MovementState.Idle:
                characterAnimator.SetAnimation("BattleRunForward", false);
                break;
            case MovementState.Running:
                characterAnimator.SetAnimation("BattleRunForward", true);
                break;
        }
    }

    private int GetCurrentMovementSpeed()
    {
        switch (currentState)
        {
            case MovementState.Running:
                return characterMotor.RunSpeed;
            default:
                return 0;
        }
    }
}
