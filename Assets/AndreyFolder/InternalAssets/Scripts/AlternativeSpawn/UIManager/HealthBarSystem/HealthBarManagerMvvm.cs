using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HealthBarManagerMvvm : MonoCache
{
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Camera _cam;
    [SerializeField] private bool _autoExpand;

    private Dictionary<IEnemy, HealthBarView> _objectToHealthBarMap = new Dictionary<IEnemy, HealthBarView>();
    private PoolObject<HealthBarView> _healthBarPool;
    private HealthBarView _prefab;
    private HealthBarController _enemyHealthBarController;
    private Action<GameObject> objectCreated;
    private Action<GameObject> objectReturnToPool;

    [Inject] private DiContainer _diContainer;
    private void Start()
    {
        Invoke("InitCamera", 0.5f);
        _prefab = _healthBarPrefab.GetComponent<HealthBarView>();
        InitPool();
        objectCreated = UIEventManager.Instance.CreateHealthBar;
        objectReturnToPool = UIEventManager.Instance.DestroyHealthBar;
        EnemyEventManager.Instance.OnObjectSetActive += HandleObjectSetActive;
    }

    private void InitCamera()
    {
        _cam = Camera.main;
    }

    private void InitPool()
    {
        PoolObject<HealthBarView>.CreateInstance(_prefab,
            _canvasTransform, "HealthBarsContainer", _diContainer);
        _healthBarPool = PoolObject<HealthBarView>.Instance;
    }

    protected override void OnDisabled()
    {
        if (EnemyEventManager.Instance != null)
        {
            EnemyEventManager.Instance.OnObjectSetActive -= HandleObjectSetActive;
        }
    }

    protected override void Run()
    {
        // Update the positions of the health bars to match the positions of the objects
        foreach (KeyValuePair<IEnemy, HealthBarView> entry in _objectToHealthBarMap)
        {
            IEnemy obj = entry.Key;
            HealthBarView healthBarView = entry.Value;
            healthBarView._healthBar.transform.position = obj.BaseEnemy.transform.position;

            Vector3 directionToCamera = _cam.transform.forward + healthBarView._healthBar.transform.position;
            healthBarView._healthBar.Fill.transform.LookAt(directionToCamera);
            healthBarView._healthBar.Border.transform.LookAt(directionToCamera);
        }
    }

    private void HandleObjectSetActive(IEnemy obj)
    {
        if (obj == null)
        {
            // Handle the case where the object reference is null
            return;
        }
        
        if (obj.BaseEnemy.gameObject.activeSelf && obj.BaseEnemy.gameObject.TryGetComponent(out IEnemy enemy))
        {
            
            // If the object is being activated, create a new health bar view model and add it to the map
            HealthBarView healthBarView = _healthBarPool.GetObjects(transform.position, _prefab, _autoExpand);

            objectCreated?.Invoke(healthBarView.gameObject);

            if (healthBarView != null)
            {
                _enemyHealthBarController = enemy.HealthBarController;
                _enemyHealthBarController._healthBarView = healthBarView;
                EnemyData enemyType = enemy.EnemyType;
                int maxHealth = enemyType.MaxHealth;
                _enemyHealthBarController.Initialize(maxHealth);

                healthBarView._healthBar.transform.position = Camera.main.WorldToScreenPoint(obj.BaseEnemy.transform.position);
                _objectToHealthBarMap.Add(enemy, healthBarView);
            }
        }
        else
        {
            // If the object is being deactivated, destroy its health bar view model and remove it from the map
            if (_objectToHealthBarMap.TryGetValue(obj, out HealthBarView healthBarView))
            {
                objectReturnToPool?.Invoke(healthBarView.gameObject);

                _objectToHealthBarMap.Remove(obj);
                _healthBarPool.ReturnObject(healthBarView);
            }
        }
    }
}
