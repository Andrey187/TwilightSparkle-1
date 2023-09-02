using UnityEngine;

namespace DamageNumber
{
    public class DamageAbilityNumbers : TextDamage<DamageAbilityNumbers>
    {
        private float _lastTimeDamageAbilityNumbers;
        private float _lastExecutionTime;

        protected override void OnEnabled()
        {
            _lastTimeDamageAbilityNumbers = _lifeTime;
        }
        protected override void Run()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Damage");
            float currentTime = Time.time;
            _lastTimeDamageAbilityNumbers -= Time.deltaTime;

            if (currentTime - _lastExecutionTime >= 0.1f)
            {
                _lastExecutionTime = currentTime;
                if (_lastTimeDamageAbilityNumbers <= 0f)
                {
                    TextDamageEvent.Instance.ReturnToPoolDamageText(this);
                }

                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

