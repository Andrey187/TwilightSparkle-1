using UnityEngine;

namespace DamageNumber
{
    public class DamageDotNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] public float _lifeTime = 2f;

        [SerializeField] public Vector3 _direction;

        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
        public Vector3 Direction { get => _direction; set => _direction = value; }

        public void SetNumberDirection()
        {
            _direction += new Vector3(0f, 1f, 0f);
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

