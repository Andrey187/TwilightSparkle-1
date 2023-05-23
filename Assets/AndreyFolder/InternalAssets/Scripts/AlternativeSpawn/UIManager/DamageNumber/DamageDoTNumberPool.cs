using System.Collections.Generic;
using UnityEngine;

namespace DamageNumber
{
    public class DamageDoTNumberPool : DamageNumberPoolBase<DamageDotNumbers, List<DamageDotNumbers>>
    {
        private static DamageDoTNumberPool instance;

        public static DamageDoTNumberPool Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DamageDoTNumberPool>();
                return instance;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override Color Color(object ability)
        {
            return ((IDoTEffect)ability).DoTColor;
        }

        protected internal override void Initialize(int damageAmount, Transform target, object ability)
        {
            base.Initialize(damageAmount, target, ability);
        }
    }
}

