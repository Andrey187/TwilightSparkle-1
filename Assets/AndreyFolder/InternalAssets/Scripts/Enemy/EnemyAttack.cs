using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoCache
{
    [SerializeField] private int _damageAmount;
    // Timer to track collision time
    private float _timeColliding;
    // Time before damage is taken, 3 second default
    private float timeThreshold = 3f;
    private BaseEnemy _baseEnemy;
    private GameObject _player;
    private PlayerStats _playerStats;
    private Collider _playerCollider;

    private void Start()
    {
        _baseEnemy = Get<BaseEnemy>();
        _damageAmount = _baseEnemy._enemyType.Damage;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<PlayerStats>();
        _playerCollider = _player.GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == _playerCollider)
        {
            // Reset timer
            _timeColliding = 0f;
            // Take damage on impact?
            AttackPlayer();
        }
    }

    // called each frame the collider is colliding
    private void OnTriggerExit(Collider collider)
    {
        if (collider == _playerCollider)
        {
            // If the time is below the threshold, add the delta time
            if (_timeColliding < timeThreshold)
            {
                _timeColliding += Time.deltaTime;
            }
            else
            {
                // Time is over theshold, player takes damage
                AttackPlayer();
                // Reset timer
                _timeColliding = 0f;
            }
        }
    }

    private void AttackPlayer()
    {
        _playerStats.CurrentHealth -= _damageAmount;
    }
}
