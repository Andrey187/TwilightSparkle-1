using System;
using UnityEngine;

public class MultipleFireBallAbility : BaseAbilities
{
    [SerializeField] protected int _alternativeCountAbilities;
    [SerializeField] protected LayerMask _enemyLayer;
    private static float _currentAngle = 0f;
    private MultipleFireBall _multipleFireBall;
    private FireDoTEffect _fireDotEffect;
    private Vector3 _projectileMoveDirection;
    private float _lastExecutionTime;
    private Collider[] _hitColliders;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;

    protected internal override int AlternativeCountAbilities { get => _alternativeCountAbilities; set => _alternativeCountAbilities = value; }

    protected override float LastExecutionTime { get => _lastExecutionTime; set => _lastExecutionTime = value; }

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        AudioManager.Instance.PlaySFX(Sound.SoundEnum.FireBall);
    }

    private void Start()
    {
        _multipleFireBall = MultipleFireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _multipleFireBall.CurrentAbility = _multipleFireBall;
        _multipleFireBall.DoTEffect = _fireDotEffect;
    }

    protected internal override void CalculateAndIncrementAngle(float countOfProjectiles)
    {
        float angleStep = 360f / countOfProjectiles;
        _currentAngle += angleStep;
    }

    protected internal override void CalculateAlternativeMovePosition()
    {
        float dirX = Mathf.Sin(_currentAngle * Mathf.Deg2Rad);
        float dirZ = Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        _projectileMoveDirection = new Vector3(dirX, 0, dirZ);
    }

    protected internal override void AlternativeMove()
    {
        _thisRb.velocity = _projectileMoveDirection * _speed;
    }

    protected override void Run()
    {
        UnityEngine.Profiling.Profiler.BeginSample("MyltipleBall");
        float currentTime = Time.time;
        if (currentTime - _lastExecutionTime >= 0.5f)
        {
            _lastExecutionTime = currentTime;
            PerformDamageCheck();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected override void PerformDamageCheck()
    {
        if (LifeTime <= 0)
        {
            // Clear the hitColliders array
            _hitColliders = new Collider[0];
            return; // Exit the method early without processing collisions
        }

        Vector3 currentPosition = transform.position;

        // Cast a sphere to detect enemies within the damage radius
        _hitColliders = Physics.OverlapSphere(currentPosition, damageRadius, _enemyLayer);


        foreach (Collider hitCollider in _hitColliders)
        {
            // Assuming enemies have a BaseEnemy component
            BaseEnemy enemy = hitCollider.GetComponent<BaseEnemy>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                _setDamage?.Invoke(enemy, _multipleFireBall.Damage, _multipleFireBall.CurrentAbility, _multipleFireBall.DoTEffect);
            }

            if ((_layerMaskForDie.value & (1 << hitCollider.transform.gameObject.layer)) > 0)
            {
                // If the collided object has the specified layer, invoke SetDie
                SetDie?.Invoke(this);
            }
        }
    }
}
