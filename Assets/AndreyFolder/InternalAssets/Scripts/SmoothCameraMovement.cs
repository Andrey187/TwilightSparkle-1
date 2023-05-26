using UnityEngine;

public class SmoothCameraMovement : MonoCache
{
    [SerializeField] private Transform _target; // The target to follow (e.g., character's transform)
    [SerializeField] private float _smoothSpeed = 0.125f; // The smoothness factor for camera movement
    [SerializeField] private Vector3 _offset; // The offset from the target's position

    protected override void LateRun()
    {
        // Calculate the desired position based on the target's position and offset
        Vector3 desiredPosition = _target.position + _offset;

        // Use SmoothDamp to smoothly interpolate the current position to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    protected override void Run()
    {
        // Make the camera look at the target's position
        transform.LookAt(_target);
    }
}
