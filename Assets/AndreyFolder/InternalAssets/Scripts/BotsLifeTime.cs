using UnityEngine;
using System;

public class BotsLifeTime : MonoCache, IPooledObjects
{
    public StageEvent.ObjectType Type => type;
    [SerializeField] private StageEvent.ObjectType type;
    //временные парамы для теста
    [SerializeField] private float _lifeTime = 3;
    [SerializeField] private float _currentLifeTime;

    public static Action<StageEvent.ObjectType> onBotDestroy;

    public void OnCreate(Vector3 position,  Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _currentLifeTime = _lifeTime;
    }

    protected override void Run()
    {
        if ((_currentLifeTime -= Time.deltaTime) < 0)
        {
            onBotDestroy?.Invoke(type);
            ObjectPooler.Instance.DestroyObject(gameObject);
        }
    }
}
