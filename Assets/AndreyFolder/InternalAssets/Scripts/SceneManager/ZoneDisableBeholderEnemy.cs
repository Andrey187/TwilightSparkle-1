using UnityEngine;

public class ZoneDisableBeholderEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBeholder enemyBeholder))
        {
            enemyBeholder.ReturnToPool();
        }
    }
}
