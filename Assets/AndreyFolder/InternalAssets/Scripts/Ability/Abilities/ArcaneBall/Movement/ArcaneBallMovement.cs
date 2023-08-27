using UnityEngine;

public class ArcaneBallMovement: BaseAbilityMovement
{
    [SerializeField] private int _rotationSpeed;
    [SerializeField] private int _areaRadiusFindEnemy = 10;
    private Transform _target;
    protected override void OnEnabled()
    {
        EnsureDependenciesInjected();
        Transform targetTransform = FindNearestEnemyInArea(_areaRadiusFindEnemy);

        if (targetTransform != null)
        {
            _target = targetTransform;
        }
        else
        {
            _target.position = Vector3.zero;
        }

        MoveWithPhysics(_target, _startPosition);
    }

    public Transform FindNearestEnemyInArea(float areaRadius) //метод моиска ближайшего противника
    {
        Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, areaRadius, LayerMask.GetMask("Enemy"));

        if (detectedEnemies.Length > 0)
        {
            int randomIndex = Random.Range(0, detectedEnemies.Length);
            return detectedEnemies[randomIndex].transform;
        }
        return null;
    }

    public override void MoveWithPhysics(Transform endPoint, Transform startPoint)
    {
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _thisRb.velocity = direction * _speed;
    }
}

