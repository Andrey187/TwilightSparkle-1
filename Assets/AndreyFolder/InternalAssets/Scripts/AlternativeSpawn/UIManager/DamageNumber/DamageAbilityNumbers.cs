using UnityEngine;

namespace DamageNumber
{
    public class DamageAbilityNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] private float _lifeTime = 2f;

        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _offsetXRange = 1.0f;
        [SerializeField] private float _offsetZRange = 1.0f;
        private float _offsetX;
        private float _offsetZ;
        private Animator _animator;

        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected override void OnEnabled()
        {
            _lifeTime = 2f;
            _offsetX = Random.Range(-_offsetXRange, _offsetXRange);
            _offsetZ = Random.Range(-_offsetZRange, _offsetZRange);
        }

        protected override void Run()
        {
            // Calculate the current position based on the speed and time
            float moveSpeed = _speed * _animator.speed;
            Vector3 currentPosition = transform.position;
            currentPosition.y += moveSpeed * Time.deltaTime;
            currentPosition.x += moveSpeed * Time.deltaTime + _offsetX * Time.deltaTime;
            currentPosition.z += moveSpeed * Time.deltaTime + _offsetZ * Time.deltaTime;

            // Assign the updated position to the transform of the text object
            transform.position = currentPosition;
        }
    }
}

