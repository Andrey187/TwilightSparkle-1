using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    }
}

