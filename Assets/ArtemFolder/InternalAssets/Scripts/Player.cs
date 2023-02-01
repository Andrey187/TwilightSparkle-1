using UnityEngine;
using StateMahine;
using PlayerStates;

namespace Player
{
    public class Player: MonoBehaviour
    {
        private StateMachine _SM;
        
        private PlayerIdle _playerIdleState;
        private PlayerMove _playerMoveState;

        private void Start()
        {
            InitStates();
        }

        private void Update()
        {
            _SM.CurrentState.Update();
        }

        
        // Showing how to Init states
        private void InitStates()
        {
            _SM = new StateMachine();
            
            _playerIdleState = new PlayerIdle();
            _playerMoveState = new PlayerMove();

            _SM.Initialize(_playerIdleState);
        }

        
        // Showing how to change State
        private void StartMove()
        {
            _SM.ChangeState(_playerMoveState);
        }

        // Showing how to use Method from state
        private void GetMethodFromState()
        {
            _playerMoveState.RemoveHP();
        }
    }
}