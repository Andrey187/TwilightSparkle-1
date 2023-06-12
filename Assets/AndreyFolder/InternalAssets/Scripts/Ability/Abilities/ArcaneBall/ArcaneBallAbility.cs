using System;
using UnityEngine;

public class ArcaneBallAbility : BaseAbilities
{
    [SerializeField] private int _rotationSpeed;
    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] public int _areaRadius = 10;
    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected internal override event Action<BaseAbilities> SetDie;
    protected internal override Vector3 TargetPoint { get => _targetPoint; set => _targetPoint = value; }
    protected internal override int AreaRadius { get => _areaRadius; set => _areaRadius = value; }
    protected internal override bool HasTargetPoint => true;
    private ArcaneBall _arcaneBall;

    private void Awake()
    {
        _thisRb = Get<Rigidbody>();
    }

    private void Start()
    {
        _arcaneBall = new ArcaneBall();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _arcaneBall.CurrentAbility = _arcaneBall;
    }

    protected internal override void MoveWithPhysics()
    {
        Vector3 direction = (_targetPoint - _startPoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _thisRb.velocity = direction * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the fireball is an enemy
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null )
        {
            // If so, damage the enemy and destroy the fireball
            _setDamage?.Invoke(enemy, _arcaneBall.Damage, _arcaneBall.CurrentAbility, _arcaneBall.DoTEffect);
            
        }
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {   
            SetDie?.Invoke(this);
        }
    }
}
