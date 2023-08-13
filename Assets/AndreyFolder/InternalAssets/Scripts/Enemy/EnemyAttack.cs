using UnityEngine;

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
    private MagicShield _magicShield;
    private Collider _playerCollider;

    private void Start()
    {
        _baseEnemy = ParentGet<BaseEnemy>();
        _damageAmount = _baseEnemy.EnemyType.Damage;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<PlayerStats>();
        _magicShield = _player.GetComponent<MagicShield>();
        _playerCollider = _player.GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == _playerCollider)
        {
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
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        if (_magicShield != null && _magicShield.IsShieldActive)
        {
            // Shield is active, no damage to the player
        }
        else
        {
            // Reset timer
            _timeColliding = 0f;
            // Take damage on impact?
            _playerStats.CurrentHealth -= _damageAmount;
            _baseEnemy.ChangeState(new AttackingState());
        }
    }
}
