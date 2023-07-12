using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraArea : MonoCache
{
    [SerializeField] private LayerMask _targetLayerMask;
    public List<Renderer> enemyObjectsRenderer;
    private Dictionary<Image, Renderer> _healthBarFillToEnemyMap;
    private Dictionary<Image, Renderer> _healthBarBorderToEnemyMap;
    private EnemyEventManager _enemyEventManager;
    private UIEventManager _uiEventManager;
    private void Start()
    {
        _healthBarFillToEnemyMap = new Dictionary<Image, Renderer>();
        _healthBarBorderToEnemyMap = new Dictionary<Image, Renderer>();
        _enemyEventManager = EnemyEventManager.Instance;
        _uiEventManager = UIEventManager.Instance;

        // Subscribe to the events for adding/removing objects to/from the pools
        _enemyEventManager.ObjectCreated += AddEnemyObject;
        _enemyEventManager.ObjectDestroyedOutCameraArea += RemoveEnemyObject;

        _uiEventManager.HealthBarCreated += AddHealthBar;
        _uiEventManager.HealthBarDestroyed += RemoveHealthBar;
    }

    protected override void OnDisabled()
    {
        if(_enemyEventManager != null)
        {
            _enemyEventManager.ObjectCreated -= AddEnemyObject;
            _enemyEventManager.ObjectDestroyedOutCameraArea -= RemoveEnemyObject;
        }

        if (_uiEventManager != null)
        {
            _uiEventManager.HealthBarCreated -= AddHealthBar;
            _uiEventManager.HealthBarDestroyed -= RemoveHealthBar;
        }
    }

    private void AddEnemyObject(GameObject enemyObject)
    {
        Renderer renderer = enemyObject.GetComponent<BaseEnemy>()._renderer;
        if (renderer != null)
        {
            enemyObjectsRenderer.Add(renderer);
        }
    }

    private void RemoveEnemyObject(GameObject enemyObject)
    {
        Renderer renderer = enemyObject.GetComponent<BaseEnemy>()._renderer;
        if (renderer != null)
        {
            enemyObjectsRenderer.Remove(renderer);
        }
    }

    private void AddHealthBar(GameObject _healthBar)
    {
        Image healthBarFillComponent = _healthBar.transform.GetComponentInChildren<Image>();
        _healthBarFillToEnemyMap.Add(healthBarFillComponent, null);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        _healthBarBorderToEnemyMap.Add(healthBarBorderComponent,null);

        foreach (Renderer enemyRenderer in enemyObjectsRenderer)
        {
            if (enemyRenderer.gameObject.activeInHierarchy)
            {
                _healthBarFillToEnemyMap[healthBarFillComponent] = enemyRenderer;
            }
        }
        foreach (Renderer enemyRenderer in enemyObjectsRenderer)
        {
            if (enemyRenderer.gameObject.activeInHierarchy)
            {
                _healthBarBorderToEnemyMap[healthBarBorderComponent] = enemyRenderer;
            }
        }
    }

    private void RemoveHealthBar(GameObject _healthBar)
    {
        Image healthBarFillComponent = _healthBar.transform.GetComponentInChildren<Image>();
        _healthBarFillToEnemyMap.Remove(healthBarFillComponent);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        _healthBarBorderToEnemyMap.Remove(healthBarBorderComponent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            
            if(enemy._renderer != null && enemyObjectsRenderer.Contains(enemy._renderer))
            {
                enemy._renderer.enabled = true;

                if (enemy != null)
                {
                    enemy.ResetHPTimer(); // Reset the XP change timer
                    enemy.SetShouldIncrementHPTimer(false); // Stop incrementing the timer
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == enemy._renderer)
                    {
                        SetRenderingHealthBarViewEnabled(true, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarBorderToEnemyMap)
                {
                    if (kvp.Value == enemy._renderer)
                    {
                        SetRenderingHealthBarViewEnabled(true, kvp.Key);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();

            if (enemy._renderer != null && enemyObjectsRenderer.Contains(enemy._renderer))
            {
                enemy._renderer.enabled = false;

                if (enemy != null)
                {
                    enemy.SetShouldIncrementHPTimer(true); // Start incrementing the timer
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == enemy._renderer)
                    {
                        SetRenderingHealthBarViewEnabled(false, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarBorderToEnemyMap)
                {
                    if (kvp.Value == enemy._renderer)
                    {
                        SetRenderingHealthBarViewEnabled(false, kvp.Key);
                    }
                }
            }
        }
    }

    private void SetRenderingHealthBarViewEnabled(bool enabled, Image image)
    {
        image.enabled = enabled;
    }
}
