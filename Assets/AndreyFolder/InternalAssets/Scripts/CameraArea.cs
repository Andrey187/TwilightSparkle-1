using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraArea : MonoCache
{
    [SerializeField] private LayerMask targetLayerMask;
    public List<Renderer> enemyObjectsRenderer;
    private HashSet<Image> healthBarFill;
    private HashSet<Image> healthBarBorder;
    private Dictionary<Image, Renderer> _healthBarFillToEnemyMap;
    private Dictionary<Image, Renderer> _healthBarBorderToEnemyMap;
    private EventManager _eventManager;

    private void Start()
    {
        healthBarFill = new HashSet<Image>();
        healthBarBorder = new HashSet<Image>();
        _healthBarFillToEnemyMap = new Dictionary<Image, Renderer>();
        _healthBarBorderToEnemyMap = new Dictionary<Image, Renderer>();
        _eventManager = EventManager.Instance;

        // Subscribe to the events for adding/removing objects to/from the pools
        _eventManager.ObjectCreated += AddEnemyObject;
        _eventManager.ObjectDestroyed += RemoveEnemyObject;

        _eventManager.HealthBarCreated += AddHealthBar;
        _eventManager.HealthBarDestroyed += RemoveHealthBar;
    }
    protected override void OnDisabled()
    {
        // Unsubscribe from the events to avoid memory leaks
        _eventManager.ObjectCreated -= AddEnemyObject;
        _eventManager.ObjectDestroyed -= RemoveEnemyObject;

        _eventManager.HealthBarCreated -= AddHealthBar;
        _eventManager.HealthBarDestroyed -= RemoveHealthBar;
    }

    private void AddEnemyObject(GameObject enemyObject)
    {
        Renderer renderer = enemyObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            enemyObjectsRenderer.Add(renderer);
        }
    }

    private void RemoveEnemyObject(GameObject enemyObject)
    {
        Renderer renderer = enemyObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            enemyObjectsRenderer.Remove(renderer);
        }
    }

    private void AddHealthBar(GameObject _healthBar)
    {
        Image healthBarFillComponent = _healthBar.transform.GetComponentInChildren<Image>();
        healthBarFill.Add(healthBarFillComponent);
        _healthBarFillToEnemyMap.Add(healthBarFillComponent, null);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        healthBarBorder.Add(healthBarBorderComponent);

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
        healthBarFill.Remove(healthBarFillComponent);
        _healthBarFillToEnemyMap.Remove(healthBarFillComponent);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        healthBarBorder.Remove(healthBarBorderComponent);
        _healthBarBorderToEnemyMap.Remove(healthBarBorderComponent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetLayerMask == (targetLayerMask | (1 << other.gameObject.layer)))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null && enemyObjectsRenderer.Contains(renderer))
            {
                renderer.enabled = true;
                BaseEnemy enemy = renderer.GetComponent<BaseEnemy>();

                if (enemy != null)
                {
                    enemy.ResetXPTimer(); // Reset the XP change timer
                    enemy.SetShouldIncrementXPTimer(false); // Stop incrementing the timer
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == renderer)
                    {
                        SetRenderingHealthBarViewEnabled(true, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarBorderToEnemyMap)
                {
                    if (kvp.Value == renderer)
                    {
                        SetRenderingHealthBarViewEnabled(true, kvp.Key);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetLayerMask == (targetLayerMask | (1 << other.gameObject.layer)))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null && enemyObjectsRenderer.Contains(renderer))
            {
                renderer.enabled = false;

                BaseEnemy enemy = renderer.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.SetShouldIncrementXPTimer(true); // Start incrementing the timer
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == renderer)
                    {
                        SetRenderingHealthBarViewEnabled(false, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, Renderer> kvp in _healthBarBorderToEnemyMap)
                {
                    if (kvp.Value == renderer)
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
