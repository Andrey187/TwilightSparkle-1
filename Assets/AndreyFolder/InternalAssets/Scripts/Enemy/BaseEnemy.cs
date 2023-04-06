using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(Mesh), typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoCache
{
    [SerializeField] protected internal EnemyType _enemyType;
    [SerializeField] protected internal int _currentHealth;
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected NavMeshAgent _navMeshAgent;

    private protected void Awake()
    {
        _meshFilter = Get<MeshFilter>();
        _meshRenderer = Get<MeshRenderer>();
        _navMeshAgent = Get<NavMeshAgent>();
        _meshFilter.mesh = _enemyType.Mesh;
        _meshRenderer.material = _enemyType.Material;

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

    public virtual void Die()
    {
        Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
        setObjectActive?.Invoke(gameObject, false);
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
