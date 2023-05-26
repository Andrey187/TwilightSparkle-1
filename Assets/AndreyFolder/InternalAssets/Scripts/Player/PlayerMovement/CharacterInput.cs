using UnityEngine;

public class CharacterInput : MonoCache
{
    private CharacterMovement characterMovement;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    protected override void FixedRun()
    {
        // Read input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Pass the input to the CharacterMovement script
        characterMovement.HandleInput(horizontal, vertical);
    }
}
