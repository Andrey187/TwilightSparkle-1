using UnityEngine;
using Zenject;

public class BaseDrop : MonoCache
{
    [SerializeField] protected LayerMask _targetLayerMask;
    protected Transform _player;
    [Inject]protected IPlayerStats _playerStats;

    protected virtual void Start()
    {
        _player = FindObjectOfType<CharacterMovement>().transform;
    }
}
