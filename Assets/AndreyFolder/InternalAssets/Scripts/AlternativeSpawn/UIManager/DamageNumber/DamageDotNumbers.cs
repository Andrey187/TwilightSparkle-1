using UnityEngine;

namespace DamageNumber
{
    public class DamageDotNumbers : TextDamage<DamageDotNumbers>
    {
        private float _lastTimeDamageDotNumbers;
        private float _lastExecutionTime;
        protected override void OnEnabled()
        {
            _lastTimeDamageDotNumbers = _lifeTime;
        }

        protected override void Run()
        {
            UnityEngine.Profiling.Profiler.BeginSample("DoT");

            _lastTimeDamageDotNumbers -= Time.deltaTime;
            float currentTime = Time.time;
            if (currentTime - _lastExecutionTime >= 0.1f)
            {
                _lastExecutionTime = currentTime;
                if (_lastTimeDamageDotNumbers <= 0f)
                {
                    TextDamageEvent.Instance.ReturnToPoolDamageDotNumbersText(this);
                }
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

