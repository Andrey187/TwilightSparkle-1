using UnityEngine;
using System.Collections.Generic;

namespace DamageNumber
{
    public class DamageNumberPool : DamageNumberPoolBase<DamageAbilityNumbers, List<DamageAbilityNumbers>>
    {
        private static DamageNumberPool instance;

        public static DamageNumberPool Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DamageNumberPool>();
                return instance;
            }
        }

        protected override void Start()
        {
            base.Start();
            TextDamageEvent.Instance.ReturnToPoolEventDamageText += InitializeReturnToPool;
        }

        protected override Color Color(object ability)
        {
            return ((IAbility)ability).Color;
        }

        protected internal override void InitializeGetObjectFromPool(int damageAmount, Transform target, object ability)
        {
            GetObjectFromPool(damageAmount, target, ability);
        }

        protected override void InitializeReturnToPool(DamageAbilityNumbers component)
        {
            ReturnToPool(component);
        }

        protected override void UnsubscribeEvents()
        {
            TextDamageEvent.Instance.ReturnToPoolEventDamageText -= InitializeReturnToPool;
        }
    }
}
