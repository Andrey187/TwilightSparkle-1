using UnityEngine;
using UnityEngine.Profiling;

public class BlockRotation : MonoCache
{
    public Transform target; // Reference to the character's transform
    public Vector3 offset; // Offset from the character's position

    protected override void Run()
    { 
        // Check if the target and offset are set
        if (target != null)
        {
            // Calculate the new position based on the target's position and the offset
            Vector3 newPosition = target.position + offset;

            // Move the object to the new position
            transform.position = newPosition;
        }
    }
}
