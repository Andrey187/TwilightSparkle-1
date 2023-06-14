using System;
using UnityEngine;

public class ExplosionAbility : BaseAbilities
{
    [SerializeField] private float _damageRadius;

    private Explosion _explosion;
    private FireDoTEffect _fireDotEffect;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    private void Start()
    {
        _explosion = new Explosion();
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _explosion.CurrentAbility = _explosion;
        _explosion.DoTEffect = _fireDotEffect;
    }

    public void Explode(bool hitGround)
    {
        if (hitGround)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius);
            foreach (Collider collider in colliders)
            {
                if(_layerMask == (_layerMask | (1 << collider.gameObject.layer)))
                {
                    BaseEnemy enemy = collider.GetComponent<BaseEnemy>();
                    if (enemy != null)
                    {
                        // If so, damage the enemy and destroy the fireball
                        _setDamage?.Invoke(enemy, _explosion.Damage, _explosion.CurrentAbility, _explosion.DoTEffect);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
