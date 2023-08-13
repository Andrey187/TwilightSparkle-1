using System;
using UnityEngine;

public class MeteorDrop : BaseDrop
{
    [SerializeField] private MeteorSpawner _meteorSpawner;
    [SerializeField] private ParticleSystem _particleSystem;
    protected override void Start()
    {
        base.Start();
        _meteorSpawner = Find<MeteorSpawner>();
        _particleSystem.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            _particleSystem.Stop();

            Action pickUpInvoke = DropEventManager.Instance.MeteorPickUp;
            pickUpInvoke?.Invoke();

            StartCoroutine(_meteorSpawner.SpawnMeteor());
            Invoke("ReturnToPool", 2);
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
