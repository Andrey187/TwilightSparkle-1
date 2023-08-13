using UnityEngine;

namespace DamageNumber
{
    public class DamageAbilityNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] private float _lifeTime = 1f;
        [SerializeField] private Rigidbody _thisRb;

        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

        private float _lastVelocityResetTime;
        private float _velocityResetInterval = 1f; // Reset velocity every 0.5 seconds

        protected override void Run()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Damage");
            float currentTime = Time.time;
            if (currentTime - _lastVelocityResetTime >= _velocityResetInterval)
            {
                _lastVelocityResetTime = currentTime;
                _thisRb.velocity = Vector3.up * 1;
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

