using System;
using UnityEngine;

public class ExplosionAbility : BaseAbilities
{
    [SerializeField] private float _damageRadius;

    private Explosion _explosion;
    private FireDoTEffect _fireDotEffect;
    protected override event Action<IEnemy, int, IAbility, IDoTEffect> _setDamageIEnemy;
    private Collider[] _collidersBuffer;

    private void Start()
    {
        _explosion = new Explosion();
        _fireDotEffect = new FireDoTEffect();
        _collidersBuffer = new Collider[32];
        _setDamageIEnemy = AbilityEventManager.Instance.AbillityDamageIEnemy;
        _explosion.CurrentAbility = _explosion;
        _explosion.DoTEffect = _fireDotEffect;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        if (_collidersBuffer != null)
        {
            Array.Clear(_collidersBuffer, 0, _collidersBuffer.Length);
        }
    }

    public void Explode(bool hitGround)
    {
        if (hitGround)
        {
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _damageRadius, _collidersBuffer);
            for (int i = 0; i < colliderCount; i++)
            {
                if (_collidersBuffer[i].TryGetComponent(out IEnemy hitEnemy))
                {
                    // Damage the enemy
                    _setDamageIEnemy?.Invoke(hitEnemy, _explosion.Damage, _explosion.CurrentAbility, _explosion.DoTEffect);
                }
            }
        }
    }
}
