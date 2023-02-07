using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotsLifeTime : MonoBehaviour, IPooledObjects
{
    public ObjectPooler.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPooler.ObjectInfo.ObjectType type;

    //временные парамы для теста
    [SerializeField] private float _lifeTime = 3;
    [SerializeField] private float _currentLifeTime;

    public void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _currentLifeTime = _lifeTime;
    }

    private void Update()
    {
        if ((_currentLifeTime -= Time.deltaTime) < 0)
            ObjectPooler.Instance.DestroyObject(gameObject);
    }
}
