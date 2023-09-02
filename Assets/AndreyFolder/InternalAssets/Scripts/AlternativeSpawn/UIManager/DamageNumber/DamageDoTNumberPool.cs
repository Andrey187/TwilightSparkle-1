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
            TextDamageEvent.Instance.ReturnToPoolEventDamageDotNumbersText += InitializeReturnToPool;
        }

        protected override Color Color(object ability)
        {
            return ((IDoTEffect)ability).DoTColor;
        }

        protected internal override void InitializeGetObjectFromPool(int damageAmount, Transform target, object ability)
        {
            GetObjectFromPool(damageAmount, target, ability);
        }

        protected override void InitializeReturnToPool(DamageDotNumbers component)
        {
            ReturnToPool(component);
        }

        protected override void UnsubscribeEvents()
        {
            TextDamageEvent.Instance.ReturnToPoolEventDamageDotNumbersText -= InitializeReturnToPool;
        }
    }
}

