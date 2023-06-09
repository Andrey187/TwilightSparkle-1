using UnityEngine;

public class BaseDrop : MonoCache
{
    [SerializeField] protected LayerMask _targetLayerMask;
    protected Transform _player;
    protected PlayerStats _playerStats;

    protected virtual void Start()
    {
        _player = FindObjectOfType<CharacterMovement>().transform;
        _playerStats = _player.GetComponentInParent<PlayerStats>();
    }
}
