using UnityEngine;

public class BotsLifeTime : MonoCache
{
    //временные парамы для теста
    [SerializeField] private float _lifeTime = 3;
    [SerializeField] private float _currentLifeTime;

    public void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _currentLifeTime = _lifeTime;
    }

    protected override void Run()
    {
        if ((_currentLifeTime -= Time.deltaTime) < 0)
        {
            ObjectPooler.Instance.DestroyObject(gameObject);
        }
    }
}
