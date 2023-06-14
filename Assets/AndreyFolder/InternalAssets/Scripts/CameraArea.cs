using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraArea : MonoCache
{
    [SerializeField] private LayerMask _targetLayerMask;
    public List<SkinnedMeshRenderer> enemyObjectsRenderer;
    private HashSet<Image> healthBarFill;
    private HashSet<Image> healthBarBorder;
    private Dictionary<Image, SkinnedMeshRenderer> _healthBarFillToEnemyMap;
    private Dictionary<Image, SkinnedMeshRenderer> _healthBarBorderToEnemyMap;
    private EnemyEventManager _enemyEventManager;
    private UIEventManager _uiEventManager;
    private void Start()
    {
        healthBarFill = new HashSet<Image>();
        healthBarBorder = new HashSet<Image>();
        _healthBarFillToEnemyMap = new Dictionary<Image, SkinnedMeshRenderer>();
        _healthBarBorderToEnemyMap = new Dictionary<Image, SkinnedMeshRenderer>();
        _enemyEventManager = EnemyEventManager.Instance;
        _uiEventManager = UIEventManager.Instance;

        // Subscribe to the events for adding/removing objects to/from the pools
        _enemyEventManager.ObjectCreated += AddEnemyObject;
        _enemyEventManager.ObjectDestroyed += RemoveEnemyObject;

        _uiEventManager.HealthBarCreated += AddHealthBar;
        _uiEventManager.HealthBarDestroyed += RemoveHealthBar;
    }
    protected override void OnDisabled()
    {
        // Unsubscribe from the events to avoid memory leaks
        _enemyEventManager.ObjectCreated -= AddEnemyObject;
        _enemyEventManager.ObjectDestroyed -= RemoveEnemyObject;

        _uiEventManager.HealthBarCreated -= AddHealthBar;
        _uiEventManager.HealthBarDestroyed -= RemoveHealthBar;
    }

    private void AddEnemyObject(GameObject enemyObject)
    {
        SkinnedMeshRenderer renderer = enemyObject.GetComponent<BaseEnemy>()._skinnedMesh;
        if (renderer != null)
        {
            enemyObjectsRenderer.Add(renderer);
        }
    }

    private void RemoveEnemyObject(GameObject enemyObject)
    {
        SkinnedMeshRenderer renderer = enemyObject.GetComponent<BaseEnemy>()._skinnedMesh;
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

        foreach (SkinnedMeshRenderer enemyRenderer in enemyObjectsRenderer)
        {
            if (enemyRenderer.gameObject.transform.parent.gameObject.activeInHierarchy)
            {
                _healthBarFillToEnemyMap[healthBarFillComponent] = enemyRenderer;
            }
        }

        foreach (SkinnedMeshRenderer enemyRenderer in enemyObjectsRenderer)
        {
            if (enemyRenderer.gameObject.transform.parent.gameObject.activeInHierarchy)
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
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            
            if(enemy._skinnedMesh != null && enemyObjectsRenderer.Contains(enemy._skinnedMesh))
            {
                enemy._skinnedMesh.enabled = true;

                if (enemy != null)
                {
                    enemy.ResetHPTimer(); // Reset the XP change timer
                    enemy.SetShouldIncrementHPTimer(false); // Stop incrementing the timer
                }

                foreach (KeyValuePair<Image, SkinnedMeshRenderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == enemy._skinnedMesh)
                    {
                        SetRenderingHealthBarViewEnabled(true, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, SkinnedMeshRenderer> kvp in _healthBarBorderToEnemyMap)
                {
                    if (kvp.Value == enemy._skinnedMesh)
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
            SkinnedMeshRenderer renderer = other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            
            if (renderer != null && enemyObjectsRenderer.Contains(renderer))
            {
                renderer.enabled = false;

                BaseEnemy enemy = renderer.GetComponentInParent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.SetShouldIncrementHPTimer(true); // Start incrementing the timer
                }

                foreach (KeyValuePair<Image, SkinnedMeshRenderer> kvp in _healthBarFillToEnemyMap)
                {
                    if (kvp.Value == renderer)
                    {
                        SetRenderingHealthBarViewEnabled(false, kvp.Key);
                    }
                }

                foreach (KeyValuePair<Image, SkinnedMeshRenderer> kvp in _healthBarBorderToEnemyMap)
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
