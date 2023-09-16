using FSG.MeshAnimator;
using UnityEngine;

public interface IEnemy
{
    public EnemyData EnemyType { get; }
    public BaseEnemy BaseEnemy { get; }

    public MeshRenderer MeshRenderer { get; }

    public MeshAnimator MeshAnimator { get; }

    public HealthBarController HealthBarController { get; }

    public void ChangeState(EnemyState newState) { }
}
