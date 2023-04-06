using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoCache
{
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Transform _cam;

    private Dictionary<BaseEnemy, HealthBar> _objectToHealthBarMap = new Dictionary<BaseEnemy, HealthBar>();
    private PoolObject<HealthBar> _healthBarPool;
    private HealthBar _prefab;

    private void Start()
    {
        _prefab = _healthBarPrefab.GetComponent<HealthBar>();
        PoolObject<HealthBar>.CreateInstance(_prefab, 15,
            _canvasTransform, "HealthBarsContainer");
        _healthBarPool = PoolObject<HealthBar>.Instance;
        EventManager.Instance.OnObjectSetActive += HandleObjectSetActive;
        EventManager.Instance.TakeDamage += TakeDamage;
    }

    protected override void Run()
    {
        // Update the positions of the health bars to match the positions of the objects

        foreach (KeyValuePair<BaseEnemy, HealthBar> entry in _objectToHealthBarMap)
        {
            BaseEnemy obj = entry.Key;
            HealthBar healthBar = entry.Value;
            healthBar.transform.position = obj.transform.position;

            Vector3 directionToCamera = _cam.forward + healthBar.transform.position;
            healthBar.Fill.transform.LookAt(directionToCamera);
            healthBar.Border.transform.LookAt(directionToCamera);
        }
    }

    private void HandleObjectSetActive(GameObject obj)
    {
        BaseEnemy enemy = obj.GetComponent<BaseEnemy>();
        if (obj.activeSelf)
        {
            // If the object is being activated, create a new health bar and add it to the map
            HealthBar healthBar = _healthBarPool.GetObjects(transform.position, _prefab);
            if (healthBar != null)
            {
                EnemyType enemyType = enemy._enemyType;
                int maxHealth = enemyType.MaxHealth;
                healthBar.SetMaxHealth(maxHealth);
                _objectToHealthBarMap.Add(enemy, healthBar);
                healthBar.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
            }
        }
        else
        {
            // If the object is being deactivated, destroy its health bar and remove it from the map
            if (_objectToHealthBarMap.TryGetValue(enemy, out HealthBar healthBar))
            {
                _objectToHealthBarMap.Remove(enemy);
                _healthBarPool.ReturnObject(healthBar);
            }
        }
    }

    private void TakeDamage(int amount)
    {
        List<BaseEnemy> toRemove = new List<BaseEnemy>();
        foreach (KeyValuePair<BaseEnemy, HealthBar> entry in _objectToHealthBarMap)
        {
            BaseEnemy obj = entry.Key;
            HealthBar healthBar = entry.Value;
            obj._currentHealth -= amount;
            healthBar.SetHealth(obj._currentHealth);

            if(amount > 0)
            {
                DamageNumberPool.Instance.Initialize(amount, healthBar.transform, Color.black);
            }

            if (obj._currentHealth <= 0)
            {
                toRemove.Add(obj);
            }
        }

        foreach (BaseEnemy enemy in toRemove)
        {
            enemy.Die();
        }
    }
}
