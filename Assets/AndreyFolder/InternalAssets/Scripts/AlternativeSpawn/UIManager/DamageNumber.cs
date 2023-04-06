using UnityEngine;

public class DamageNumber : MonoCache
{
    [SerializeField] public float _duration = 1f;
    [SerializeField] public float _upwardForce = 1f;
    [SerializeField] public float _sideForce = 1f;

    [SerializeField] public Vector3 _direction;

    protected override void FixedRun()
    {
        transform.position += _direction * Time.fixedDeltaTime * _sideForce + Vector3.up * Time.fixedDeltaTime * _upwardForce;
    }
}
