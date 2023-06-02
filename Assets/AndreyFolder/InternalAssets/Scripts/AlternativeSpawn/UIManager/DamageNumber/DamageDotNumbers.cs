using UnityEngine;
using DG.Tweening;

namespace DamageNumber
{
    public class DamageDotNumbers : MonoCache, IDamageNumber
    {
        [SerializeField] private float _lifeTime = 2f;

        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

        protected override void OnEnabled()
        {
            _lifeTime = 2f;
            transform.DOLocalMoveY(2, _lifeTime);
        }
        protected override void OnDisabled()
        {
            transform.DOPause();
        }
    }
}

