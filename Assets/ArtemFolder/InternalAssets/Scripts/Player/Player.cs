using UnityEngine;
using StateMachineSpace;
using PlayerStates;

namespace PlayerSpace
{
    [RequireComponent(typeof(PlayerComponents))]
    public class Player: MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        private PlayerComponents _playerComponents;
        
        
        private StateMachine _SM;
        private PlayerIdle _playerIdleState;
        private PlayerMove _playerMoveState;

        
        private void Start()
        {
            GetComponents();
            InitStates();
        }

        private void Update()
        {
            _SM.CurrentState.Update();
        }
        
        private void InitStates()
        {
            _SM = new StateMachine();
            
            _playerIdleState = new PlayerIdle();
            _playerMoveState = new PlayerMove(playerSettings);

            _SM.Initialize(_playerIdleState);
        }

        private void GetComponents()
        {
            _playerComponents = GetComponent<PlayerComponents>();
        }
        
        private void StartMove()
        {
            _SM.ChangeState(_playerMoveState);
        }
        
    }
}