using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using DamageNumber;

[RequireComponent(typeof(MeshFilter), typeof(Mesh), typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoCache
{
    [SerializeField] protected internal EnemyType _enemyType;
    [SerializeField] protected internal int _currentHealth;
    protected HealthBarModel _healthBarModel;
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected NavMeshAgent _navMeshAgent;

    private protected void Awake()
    {
        _healthBarModel = Get<HealthBarModel>();
        _meshFilter = Get<MeshFilter>();
        _meshRenderer = Get<MeshRenderer>();
        _navMeshAgent = Get<NavMeshAgent>();
        _meshFilter.mesh = _enemyType.Mesh;
        _meshRenderer.material = _enemyType.Material;
        EventManager.Instance.TakeAbilityDamage += TakeDamage;
        NavMeshParams();
        ColliderSelection();
    }

    protected override void OnEnabled()
    {
        if (_enemyType != null)
        {
            _enemyType.SetCurrentHealthToMax();
            _currentHealth = _enemyType.CurrentHealth;
        }
    }

    public void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void TakeDamage(int damageAmount, IAbility ability, IDoTEffect doTEffect)
    {
        if (!gameObject.activeSelf)
        {
            return; // don't apply damage or show damage numbers if the game object is not active
        }

        _currentHealth = ability.ApplyDamage(_currentHealth, damageAmount);
        _healthBarModel.CurrentHealth = _currentHealth;
        DamageNumberPool.Instance.Initialize(damageAmount, gameObject.transform, ability);

        if (gameObject != null && damageAmount > 0 && _currentHealth > 0)
        {
            if (ability.HasDoT)
            {
                StartCoroutine(PeriodicDamageCoroutine(
                    doTEffect,
                    gameObject.transform,
                    doTEffect.Duration,
                    doTEffect.TickInterval,
                    damageAmount));
            }
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator PeriodicDamageCoroutine(IDoTEffect doTEffect,
        Transform target, float duration, float interval, int amount)
    {
        float timeLeft = duration;
        int periodicDamageAmount = doTEffect.ApplyDoT(_currentHealth, amount);

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(interval);
            _currentHealth -= periodicDamageAmount;
            if (_currentHealth <= 0)
            {
                Die();
                yield break; // exit the coroutine if the enemy is dead
            }

            DamageDoTNumberPool.Instance.Initialize(periodicDamageAmount, target.transform, doTEffect);
            _healthBarModel.CurrentHealth = _currentHealth;
            timeLeft -= interval;
        }
    }

    public virtual void Die()
    {
        Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
        setObjectActive?.Invoke(gameObject, false);
        LevelUpSystem.Instance.AddExperience(_enemyType._type, _enemyType);
    }

    private void NavMeshParams()
    {
        _navMeshAgent.baseOffset = 0.5f;
        _navMeshAgent.speed = _enemyType.Speed;
        _navMeshAgent.radius = 0.5f;
        _navMeshAgent.height = 0.5f;
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    private void ColliderSelection()
    {
        if (_meshFilter != null)
        {
            if (_meshFilter.mesh != null)
            {
                if (IsMeshCube(_meshFilter.mesh))
                {
                    gameObject.AddComponent<BoxCollider>();
                    gameObject.AddComponent<SphereCollider>();
                }
                else if (IsMeshSphere(_meshFilter.mesh))
                {
                    gameObject.AddComponent<SphereCollider>();
                }
            }
        }
    }

    private bool IsMeshCube(Mesh mesh)
    {
        // check if the mesh is named "Cube"
        return mesh.name.Contains("Cube Instance");
    }

    private bool IsMeshSphere(Mesh mesh)
    {
        // check if the mesh is named "Sphere"
        return mesh.name.Contains("Sphere Instance");
    }
}
