using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilitiesSpawn : MonoCache
    {
        [SerializeField] private AttackSystem _attackSystem;
        public event Action InitializationComplete;
        private PoolObject<BaseAbilities> _abilityPool;
        private IObjectFactory _objectFactory;
        private List<BaseAbilities> activeAbilities = new List<BaseAbilities>();
        private List<BaseAbilities> expiredAbilities = new List<BaseAbilities>();
        private float _lastExecutionTime;

        private void Start()
        {
            Invoke("StartInitAbility", 2f);
        }

        private void StartInitAbility()
        {
            InitPool();
            _attackSystem.SetCreatePrefabAbility(CreatePrefabAbility);
            _attackSystem.SetCreateAlternativeAbility(CreateAlternativeAbility);
            InitializationComplete?.Invoke(); //вызов подписанного метода FirstAbilitySpawnStart
        }

        private void InitPool()
        {
            foreach (var ability in _attackSystem.AttackAbilitiesList)
            {
                _objectFactory = new ObjectsFactory(ability.GetComponent<BaseAbilities>().transform);
                BaseAbilities baseAbilities = _objectFactory.CreateObject(_attackSystem.StartAttackPoint.position).GetComponent<BaseAbilities>();
                PoolObject<BaseAbilities>.CreateInstance(baseAbilities, 0, gameObject.transform, baseAbilities.name + "_Ability");
                _abilityPool = PoolObject<BaseAbilities>.Instance;
            }
        }

        private void CreatePrefabAbility(BaseAbilities ability, Vector3 targetPoint)
        {
            BaseAbilities prefabAbility = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);
            if (ability.HasTargetPoint)
            {
                prefabAbility.TargetPoint = targetPoint;
                prefabAbility.MoveWithPhysics(_attackSystem.EndAttackPoint, _attackSystem.StartAttackPoint);
            }
            else
            {
                prefabAbility.MoveWithPhysics(_attackSystem.EndAttackPoint, _attackSystem.StartAttackPoint);
            }
           
            prefabAbility.SetDie += ReturnObjectToPool;
            activeAbilities.Add(prefabAbility); // Add to the list
        }

        private void CreateAlternativeAbility(BaseAbilities ability)
        {
            BaseAbilities[] prefabAbilities = new BaseAbilities[ability.AlternativeCountAbilities];

            for (int i = 0; i < ability.AlternativeCountAbilities; i++)
            {
                prefabAbilities[i] = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);
                prefabAbilities[i].CalculateAlternativeMovePosition();
                prefabAbilities[i].AlternativeMove();
                prefabAbilities[i].SetDie += ReturnObjectToPool;

                activeAbilities.Add(prefabAbilities[i]); // Add to the list

                prefabAbilities[i].CalculateAndIncrementAngle(ability.AlternativeCountAbilities);
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