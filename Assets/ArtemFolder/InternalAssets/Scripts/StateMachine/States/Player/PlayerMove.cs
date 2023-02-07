using StateMachineSpace;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

using PlayerSpace;

namespace PlayerStates
{
    public class PlayerMove : State
    {
        private PlayerSettings _playerSettings;
        
        private float _moveSpeed;
        private float _rotationSpeed;

        
        public PlayerMove(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
            
            _moveSpeed = _playerSettings.moveSpeed;
            _rotationSpeed = _playerSettings.rotationSpeed;
        }
        
        public override void Enter()
        {
            
        }
        
        public override void Exit()
        {
            
        }
        
        public override void Update()
        {
          
        }
    }
}


