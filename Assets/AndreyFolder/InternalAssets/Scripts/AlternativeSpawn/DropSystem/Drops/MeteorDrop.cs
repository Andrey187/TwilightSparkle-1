using UnityEngine;

public class MeteorDrop : BaseDrop
{
    [SerializeField] private MeteorSpawner _meteorSpawner;
    private MeshRenderer _render;
    protected override void Start()
    {
        base.Start();
        _meteorSpawner = Find<MeteorSpawner>();
        _render = Get<MeshRenderer>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            StartCoroutine(_meteorSpawner.SpawnMeteor());
            _render.enabled = false;
            Invoke("ReturnToPool", 2);
        }
    }

    private void ReturnToPool()
    {
        _render.enabled = true;
        gameObject.SetActive(false);
    }

}
