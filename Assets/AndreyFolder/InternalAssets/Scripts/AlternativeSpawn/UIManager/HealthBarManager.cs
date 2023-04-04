using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoCache
{
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Transform _canvasTransform;

    private Dictionary<GameObject, CanvasGroup> _objectToHealthBarMap = new Dictionary<GameObject, CanvasGroup>();
    private PoolObject<CanvasGroup> _healthBarPool;
    private CanvasGroup _prefab;
    
    private void Start()
    {
        _prefab = _healthBarPrefab.GetComponent<CanvasGroup>();
        PoolObject<CanvasGroup>.CreateInstance(_healthBarPrefab.GetComponent<CanvasGroup>(), 15,
            _canvasTransform, "HealthBarsContainer");
        _healthBarPool = PoolObject<CanvasGroup>.Instance;
        EventManager.Instance.OnObjectSetActive += HandleObjectSetActive;
    }

    protected override void Run()
    {
        // Update the positions of the health bars to match the positions of the objects

        foreach (KeyValuePair<GameObject, CanvasGroup> entry in _objectToHealthBarMap)
        {
            GameObject obj = entry.Key;
            CanvasGroup healthBar = entry.Value;
            healthBar.transform.position = obj.transform.position;
        }
    }

    private void HandleObjectSetActive(GameObject obj)
    {
        if (obj.activeSelf)
        {
            // If the object is being activated, create a new health bar and add it to the map
            CanvasGroup healthBar = _healthBarPool.GetObjects(transform.position, _prefab);
            if (healthBar != null)
            {
                _objectToHealthBarMap.Add(obj, healthBar);
                healthBar.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
            }
        }
        else
        {
            // If the object is being deactivated, destroy its health bar and remove it from the map
            if (_objectToHealthBarMap.TryGetValue(obj, out CanvasGroup healthBar))
            {
                _objectToHealthBarMap.Remove(obj);
                _healthBarPool.ReturnObject(healthBar);
            }
        }
    }
}
