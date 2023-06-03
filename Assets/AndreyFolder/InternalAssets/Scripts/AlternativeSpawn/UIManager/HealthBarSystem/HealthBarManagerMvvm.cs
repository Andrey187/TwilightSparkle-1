using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManagerMvvm : MonoCache
{
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Transform _cam;

    private Dictionary<BaseEnemy, HealthBarView> _objectToHealthBarMap = new Dictionary<BaseEnemy, HealthBarView>();
    private PoolObject<HealthBarView> _healthBarPool;
    private HealthBarView _prefab;
    private HealthBarController _enemyHealthBarController;

    private void Start()
    {
        _prefab = _healthBarPrefab.GetComponent<HealthBarView>();
        PoolObject<HealthBarView>.CreateInstance(_prefab, 15,
            _canvasTransform, "HealthBarsContainer");
        _healthBarPool = PoolObject<HealthBarView>.Instance;
        EventManager.Instance.OnObjectSetActive += HandleObjectSetActive;
    }

    protected override void OnDisabled()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnObjectSetActive -= HandleObjectSetActive;
        }
    }

    protected override void Run()
    {
        // Update the positions of the health bars to match the positions of the objects
        foreach (KeyValuePair<BaseEnemy, HealthBarView> entry in _objectToHealthBarMap)
        {
            BaseEnemy obj = entry.Key;
            HealthBarView healthBarView = entry.Value;
            healthBarView._healthBar.transform.position = obj.transform.position;

            Vector3 directionToCamera = _cam.forward + healthBarView._healthBar.transform.position;
            healthBarView._healthBar.Fill.transform.LookAt(directionToCamera);
            healthBarView._healthBar.Border.transform.LookAt(directionToCamera);
        }
    }

    private void HandleObjectSetActive(GameObject obj)
    {
        if (obj == null)
        {
            // Handle the case where the object reference is null
            return;
        }
        BaseEnemy enemy = obj.GetComponent<BaseEnemy>();
        if (obj.activeSelf)
        {
            // If the object is being activated, create a new health bar view model and add it to the map
            HealthBarView healthBarView = _healthBarPool.GetObjects(transform.position, _prefab);

            Action<GameObject> objectCreated = EventManager.Instance.CreateHealthBar;
            objectCreated?.Invoke(healthBarView.gameObject);

            if (healthBarView != null)
            {
                _enemyHealthBarController = enemy.Get<HealthBarController>();
                _enemyHealthBarController._healthBarView = healthBarView;
                EnemyType enemyType = enemy._enemyType;
                int maxHealth = enemyType.MaxHealth;
                _enemyHealthBarController.Initialize(maxHealth);


                healthBarView._healthBar.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
                _objectToHealthBarMap.Add(enemy, healthBarView);
            }
        }
        else
        {
            // If the object is being deactivated, destroy its health bar view model and remove it from the map
            if (_objectToHealthBarMap.TryGetValue(enemy, out HealthBarView healthBarView))
            {
                _objectToHealthBarMap.Remove(enemy);
                _healthBarPool.ReturnObject(healthBarView);

                Action<GameObject> objectReturnToPool = EventManager.Instance.DestroyHealthBar;
                objectReturnToPool?.Invoke(healthBarView.gameObject);
            }
        }
    }
}
