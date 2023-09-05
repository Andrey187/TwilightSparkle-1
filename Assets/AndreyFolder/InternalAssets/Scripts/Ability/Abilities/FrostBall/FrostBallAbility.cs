using System;
using UnityEngine;
using Zenject;

public class FrostBallAbility : BaseAbilities
{
    [SerializeField] private float _areaRadius;
    [Inject]private IMovementSpeedModifier _movementSpeedModifier;
    private FrostBall _frostBall;
    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    protected internal override event Action<BaseAbilities> SetDie;

    private Collider[] _collidersBuffer;
    private float _slowDuration = 3f; // Duration of the movement speed reduction
    private float _slowAmount = 0.5f; // Amount to reduce the movement speed by (0.5 = 50% reduction)

    protected override void OnDisabled()
    {
        base.OnDisabled();
        if(_collidersBuffer != null)
        {
            Array.Clear(_collidersBuffer, 0, _collidersBuffer.Length);
        }
    }

    private void Start()
    {
        _frostBall = new FrostBall();
        _collidersBuffer = new Collider[32];
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _frostBall.CurrentAbility = _frostBall;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the object that collided with the fireball is an enemy
        
        if (other.TryGetComponent(out IEnemy enemy))
        {
            ApplyAreaEffect(enemy);
        }
        // Check if the collided object has the specified mask layer
        if ((_layerMaskForDie.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
    }

    private void ApplyAreaEffect(IEnemy enemy)
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(enemy.BaseEnemy.transform.position, _areaRadius, _collidersBuffer);
        for (int i = 0; i < colliderCount; i++)
        {
            if (_collidersBuffer[i].TryGetComponent(out IEnemy hitEnemy) && hitEnemy != enemy)
            {
                // Damage the enemy
                _setDamageIEnemy?.Invoke(hitEnemy, _frostBall.Damage, _frostBall.CurrentAbility, _frostBall.DoTEffect);

                // Reduce the enemy's movement speed for the specified duration
                _movementSpeedModifier.ApplySpeedModifier(hitEnemy.EnemyType, hitEnemy.NavMeshAgent, _slowAmount, _slowDuration);
            }
        }
    }
}
