using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RenderingManager : MonoCache
{
    public Camera mainCamera;
    public List<Renderer> enemyObjectsRenderer;
    public HashSet<Image> healthBarFill;
    public HashSet<Image> healthBarBorder;
    private Dictionary<Image, Image> healthBarBorderMap;
    private EventManager _eventManager;

    private void Start()
    {
        healthBarFill = new HashSet<Image>();
        healthBarBorder = new HashSet<Image>();
        healthBarBorderMap = new Dictionary<Image, Image>();
        mainCamera = Camera.main;
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
        enemyObjectsRenderer.Add(enemyObject.GetComponent<Renderer>());
    }

    private void RemoveEnemyObject(GameObject enemyObject)
    {
        enemyObjectsRenderer.Remove(enemyObject.GetComponent<Renderer>());
    }

    private void AddHealthBar(GameObject _healthBar)
    {
        Image healthBarFillComponent = _healthBar.transform.GetComponentInChildren<Image>();
        healthBarFill.Add(healthBarFillComponent);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        healthBarBorder.Add(healthBarBorderComponent);

        healthBarBorderMap.Add(healthBarFillComponent, healthBarBorderComponent);
    }

    private void RemoveHealthBar(GameObject _healthBar)
    {
        Image healthBarFillComponent = _healthBar.transform.GetComponentInChildren<Image>();
        healthBarFill.Remove(healthBarFillComponent);

        Image healthBarBorderComponent = _healthBar.transform.Find("Border").GetComponent<Image>();
        healthBarBorder.Remove(healthBarBorderComponent);

        healthBarBorderMap.Remove(healthBarFillComponent);
    }

    protected override void Run()
    {
        foreach (Renderer enemyObject in enemyObjectsRenderer)
        {
            if (enemyObject.gameObject.activeInHierarchy)
            {
                bool isVisible = IsVisibleFromCamera(mainCamera, enemyObject) && mainCamera != null;
                enemyObject.enabled = isVisible;
            }
        }
        foreach (Image healthBarFillComponent in healthBarFill)
        {
            if (healthBarFillComponent.gameObject.activeInHierarchy)
            {
                bool isVisible = IsVisibleFromCamera(mainCamera, healthBarFillComponent) && mainCamera != null;
                SetRenderingHealthBarViewEnabled(isVisible, healthBarFillComponent);

                Image healthBarBorderComponent;
                if (healthBarBorderMap.TryGetValue(healthBarFillComponent, out healthBarBorderComponent))
                {
                    SetRenderingHealthBarViewEnabled(isVisible, healthBarBorderComponent);
                }
            }
        }
    }
    private bool IsVisibleFromCamera(Camera camera, Renderer obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return obj != null && GeometryUtility.TestPlanesAABB(planes, obj.bounds);
    }

    private bool IsVisibleFromCamera(Camera camera, Image image)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        Bounds bounds = new Bounds(image.transform.position, image.transform.localScale);
        return image != null && GeometryUtility.TestPlanesAABB(planes, bounds);
    }

    private void SetRenderingHealthBarViewEnabled(bool enabled, Image image)
    {
        image.enabled = enabled;
    }
}
