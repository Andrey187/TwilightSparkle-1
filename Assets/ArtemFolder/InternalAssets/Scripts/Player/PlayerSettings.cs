using UnityEngine;

namespace PlayerSpace
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObject/Player")]
    public class PlayerSettings : ScriptableObject
    {
        public int healthPoints = 5;
        
        public float moveSpeed = 10f;
        public float rotationSpeed = 4f;

        
    }
}

