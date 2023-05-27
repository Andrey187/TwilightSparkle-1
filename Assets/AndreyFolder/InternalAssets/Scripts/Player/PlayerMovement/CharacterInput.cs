using UnityEngine;

public class CharacterInput : MonoCache
{
    private CharacterMovement characterMovement;
    [SerializeField] private FixedJoystick _fixedJoystickMovement;
    [SerializeField] private FixedJoystick _fixedJoystickAim;


    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    protected override void FixedRun()
    {
        // Read input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        characterMovement.HandleInput(horizontal, vertical);

        if(_fixedJoystickMovement.isActiveAndEnabled && _fixedJoystickAim.isActiveAndEnabled)
        {
            float horizontalMovement = _fixedJoystickMovement.Horizontal;
            float verticalMovement = _fixedJoystickMovement.Vertical;
            characterMovement.HandleInput(horizontalMovement, verticalMovement);

            float horizontalAim = _fixedJoystickAim.Horizontal;
            float verticalAim = _fixedJoystickAim.Vertical;
            characterMovement.HandleAim(horizontalAim, verticalAim);
        }
    }
}
