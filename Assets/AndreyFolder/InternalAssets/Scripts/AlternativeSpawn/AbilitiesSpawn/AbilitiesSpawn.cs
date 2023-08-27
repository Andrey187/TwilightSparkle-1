using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AbilitySystem
{
    public class AbilitiesSpawn : MonoCache
    {
        [Inject] private IAttackSystem _attackSystem;
        [Inject] private DiContainer _diContainer;
        public event Action InitializationComplete;
        private PoolObject<BaseAbilities> _abilityPool;
        private IObjectFactory _objectFactory;
        private List<BaseAbilities> activeAbilities = new List<BaseAbilities>();
        private List<BaseAbilities> expiredAbilities = new List<BaseAbilities>();
        private float _lastExecutionTime;

        private void Start()
        {
            Invoke("InitPool", 2f);

            InitializationComplete?.Invoke(); //вызов подписанного метода FirstAbilitySpawnStart
        }

        private void InitPool()
        {
            foreach (var ability in _attackSystem.AttackAbilitiesList)
            {
                _objectFactory = new ObjectsFactory(ability.GetComponent<BaseAbilities>().transform);
                BaseAbilities baseAbilities = _objectFactory.CreateObject(_attackSystem.StartAttackPoint.position).GetComponent<BaseAbilities>();
                PoolObject<BaseAbilities>.CreateInstance(baseAbilities, 10, gameObject.transform, baseAbilities.name + "_Ability", _diContainer);
                _abilityPool = PoolObject<BaseAbilities>.Instance;
            }
            _attackSystem.SetCreatePrefabAbility(CreatePrefabAbility);
        }

        private void CreatePrefabAbility(BaseAbilities ability)
        {
            if (ability.IsMultiple)
            {
                BaseAbilities[] prefabAbilities = new BaseAbilities[ability.AlternativeCountAbilities];
                for (int i = 0; i < ability.AlternativeCountAbilities; i++)
                {
                    prefabAbilities[i] = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);

                    prefabAbilities[i].SetDie += ReturnObjectToPool;

                    activeAbilities.Add(prefabAbilities[i]); // Add to the list
                }
            }
            else
            {
                BaseAbilities prefabAbility = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);

                prefabAbility.SetDie += ReturnObjectToPool;
                activeAbilities.Add(prefabAbility); // Add to the list
            }
        }

        protected override void Run()
        {
            UnityEngine.Profiling.Profiler.BeginSample("AbilitySpawn");
            float currentTime = Time.time;
            if (currentTime - _lastExecutionTime >= 0.5f)
            {
                _lastExecutionTime = currentTime;
                CheckExpiredAbilities();
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void CheckExpiredAbilities()
        {
            for (int i = activeAbilities.Count - 1; i >= 0; i--)
            {
                BaseAbilities ability = activeAbilities[i];
                if (ability.LifeTime <= 0)
                {
                    expiredAbilities.Add(ability);
                    activeAbilities.RemoveAt(i);
                }
            }

            ProcessExpiredAbilities();
        }

        private void ProcessExpiredAbilities()
        {
            if (expiredAbilities.Count > 0)
            {
                for (int i = 0; i < expiredAbilities.Count; i++)
                {
                    BaseAbilities expiredAbility = expiredAbilities[i];
                    ReturnObjectToPool(expiredAbility);
                }
                expiredAbilities.Clear();
            }
        }

        private void ReturnObjectToPool(BaseAbilities expiredAbility)
        {
            _abilityPool.ReturnObject(expiredAbility);
            expiredAbility.SetDie -= ReturnObjectToPool;
        }
    }
}