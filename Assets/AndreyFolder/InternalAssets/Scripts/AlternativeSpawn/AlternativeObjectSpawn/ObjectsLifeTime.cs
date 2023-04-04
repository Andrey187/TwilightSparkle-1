using System;
using UnityEngine;

public class ObjectsLifeTime : MonoCache
{
    [SerializeField] private float _lifeTime = 3;
    [SerializeField] private float _currentLifeTime;
    private bool installed = false;

    protected override void OnEnabled()
    {
        _currentLifeTime = _lifeTime;
        if (installed) return;
        installed = true;
    }

    public void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    protected override void Run()
    {
        _currentLifeTime -= Time.deltaTime;

        if (_currentLifeTime <= 0f)
        {
            Action<GameObject, bool> setObjectActive = EventManager.Instance.SetObjectActive;
            setObjectActive?.Invoke(gameObject, false);
        }
    }
}
