using FSG.MeshAnimator;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    public EnemyData EnemyType { get; }
    public BaseEnemy BaseEnemy { get; }

    public MeshRenderer MeshRenderer { get; }

    public MeshAnimator MeshAnimator { get; }

    public NavMeshAgent NavMeshAgent { get; }

    public HealthBarController HealthBarController { get; }

    void ChangeState(EnemyState newState) { }
}
