using UnityEngine;

namespace DamageNumber
{
    public class DamageAbilityNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] private float _lifeTime = 2f;

        [SerializeField] private Vector3 _direction;

        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
        public Vector3 Direction { get => _direction; set => _direction = value; }

        public void SetNumberDirection() 
        {
            _direction += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        public void NumberReset() 
        {
            _lifeTime = 2f;
            _direction = Vector3.up;
        }

        protected override void FixedRun()
        {
            transform.position += _direction * Time.fixedDeltaTime + Vector3.up * Time.fixedDeltaTime;
        }
    }
}

