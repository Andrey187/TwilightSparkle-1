using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderingManager : MonoCache
{
    public Camera mainCamera;
    public List<Renderer> enemyObjects;
    public List<Image> healthBarFill;
    public List<Image> healthBarBorder;
    private EventManager _eventManager;

    private void Start()
    {
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
        enemyObjects.Add(enemyObject.GetComponent<Renderer>());
    }

    private void RemoveEnemyObject(GameObject enemyObject)
    {
        enemyObjects.Remove(enemyObject.GetComponent<Renderer>());
    }

    private void AddHealthBar(GameObject _healthBar)
    {
        healthBarFill.Add(_healthBar.transform.GetComponentInChildren<Image>());
        healthBarBorder.Add(_healthBar.transform.Find("Border").GetComponent<Image>());
    }

    private void RemoveHealthBar(GameObject _healthBar)
    {
        healthBarFill.Remove(_healthBar.transform.GetComponentInChildren<Image>());
        healthBarBorder.Remove(_healthBar.transform.Find("Border").GetComponent<Image>());
    }

    protected override void Run()
    {
        // Perform the visibility check and update rendering status for each object
        foreach (Renderer enemyObject in enemyObjects)
        {
            if (enemyObject.gameObject.activeInHierarchy)
            {
                if (IsVisibleFromCamera(mainCamera, enemyObject) && mainCamera != null)
                {
                    enemyObject.enabled = true;
                }
                else
                {
                    enemyObject.enabled = false;
                }
            }
        }

        foreach (Image healthBarFillComponent in healthBarFill)
        {
            if (healthBarFillComponent.gameObject.activeInHierarchy)
            {
                if (IsVisibleFromCamera(mainCamera, healthBarFillComponent) && mainCamera != null)
                {
                    SetRenderingFillEnabled(true, healthBarFillComponent);
                }
                else
                {
                    SetRenderingFillEnabled(false, healthBarFillComponent);
                }
            }
        }

        foreach (Image healthBarBorderComponent in healthBarBorder)
        {
            if (healthBarBorderComponent.gameObject.activeInHierarchy)
            {
                if (IsVisibleFromCamera(mainCamera, healthBarBorderComponent) && mainCamera != null)
                {
                    SetRenderingBorderEnabled(true, healthBarBorderComponent);
                }
                else
                {
                    SetRenderingBorderEnabled(false, healthBarBorderComponent);
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

    private void SetRenderingFillEnabled(bool enabled, Image image)
    {
        image.enabled = enabled;
    }
    private void SetRenderingBorderEnabled(bool enabled, Image image)
    {
        image.enabled = enabled;
    }
}
