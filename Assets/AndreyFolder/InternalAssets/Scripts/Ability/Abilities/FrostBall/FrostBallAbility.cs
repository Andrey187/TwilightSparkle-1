using System;
using UnityEngine;

public class FrostBallAbility : BaseAbilities
{
    [SerializeField] private float _areaRadius;
    private FrostBall _frostBall;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    private float _slowDuration = 3f; // Duration of the movement speed reduction
    private float _slowAmount = 0.5f; // Amount to reduce the movement speed by (0.5 = 50% reduction)

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }
    protected override void OnEnabled()
    {
        base.OnEnabled();
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.FrostBall);
    }

    private void Start()
    {
        _frostBall = new FrostBall();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _frostBall.CurrentAbility = _frostBall;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the object that collided with the fireball is an enemy
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            ApplyAreaEffect(enemy);
        }

        // Check if the collided object has the specified mask layer
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
    }

    private void ApplyAreaEffect(BaseEnemy enemy)
    {
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, _areaRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out BaseEnemy hitEnemy))
            {
                // Damage the enemy and reduce its movement speed
                _setDamage?.Invoke(hitEnemy, _frostBall.Damage, _frostBall.CurrentAbility, _frostBall.DoTEffect);

                // Reduce the enemy's movement speed for the specified duration
                enemy.ReduceMovementSpeed(_slowAmount, _slowDuration);
            }
        }
    }
}
