using UnityEngine;

public class Meteor : BaseDrop
{
    [SerializeField] private ParticleSystem _particleFlame;
    [SerializeField] private ParticleSystem _explosion;

    private ExplosionAbility _explosionAbility;
    private MeshRenderer render;
    private SphereCollider _collider;
    private Rigidbody _rb;
    private bool hasHitGround = false;

    private void Awake()
    {
        _explosionAbility = Get<ExplosionAbility>();
        render = Get<MeshRenderer>();
        _collider = Get<SphereCollider>();
        _rb = Get<Rigidbody>();
    }

    protected override void OnEnabled()
    {
        _particleFlame.Play();
        _rb.isKinematic = false;
        render.enabled = true;
        _collider.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            _rb.isKinematic = true;
            render.enabled = false;
            _collider.enabled = false;
            _particleFlame.Stop();
            _explosion.Play();
            hasHitGround = true;
            Invoke("ReturnToPool", 2);
            _explosionAbility.Explode(other, hasHitGround);
        }
    }

    private void ReturnToPool()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
