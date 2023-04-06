using System;
using UnityEngine;

public class DamageEnemies : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action<int> setObjectActive = EventManager.Instance.EnemyTakeDamage;
            setObjectActive?.Invoke(_damage);
        }
    }
}
