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
        }

        protected override Color Color(object ability)
        {
            return ((IAbility)ability).Color;
        }

        protected internal override void Initialize(int damageAmount, Transform target, object ability)
        {
            base.Initialize(damageAmount, target, ability);
        }
    }
}
