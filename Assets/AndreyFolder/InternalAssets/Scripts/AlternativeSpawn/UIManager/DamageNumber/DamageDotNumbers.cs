using UnityEngine;

namespace DamageNumber
{
    public class DamageDotNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] private float _lifeTime = 2f;

        [SerializeField] private Vector3 _direction;
        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

        protected override void OnEnabled()
        {
            _lifeTime = 2f;
            _direction = new Vector3(0f, 1f, 0f);
        }

        protected override void FixedRun()
        {
            transform.position += _direction * Time.fixedDeltaTime + Vector3.up * Time.fixedDeltaTime;
        }
    }
}

