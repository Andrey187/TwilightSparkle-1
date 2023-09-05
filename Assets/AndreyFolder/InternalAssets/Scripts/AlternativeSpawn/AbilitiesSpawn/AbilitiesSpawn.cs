using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AbilitySystem
{
    public class AbilitiesSpawn : MonoCache, IAbilitySpawn
    {
        [SerializeField] private List<BaseAbilities> _abilityListPool;
        [SerializeField] private int _countObjectsInPool = 50;
        [SerializeField] private bool _autoExpand;
        [Inject] private IAttackSystem _attackSystem;
        [Inject] private DiContainer _diContainer;
        public event Action InitializationComplete;
        public event Action<Sound.SoundEnum> PlaySound;

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
            List<BaseAbilities> newCachedAbility = new List<BaseAbilities>();
            for (int i = 0; i < _abilityListPool.Count; i++)
            {
                _objectFactory = new ObjectsFactory(_abilityListPool[i].GetComponent<BaseAbilities>().transform);
               
                // Add the bots to the List
                for (int j = 0; j < _countObjectsInPool; j++)
                {
                    BaseAbilities baseAbilities = _objectFactory.CreateObject(_attackSystem.StartAttackPoint.position).GetComponent<BaseAbilities>();
                    newCachedAbility.Add(baseAbilities);
                }
            }
           
            PoolObject<BaseAbilities>.CreateInstance(newCachedAbility, gameObject.transform, "_Ability", _diContainer);
            _abilityPool = PoolObject<BaseAbilities>.Instance;

            _attackSystem.SetCreatePrefabAbility(CreatePrefabAbility);
        }

        private void CreatePrefabAbility(BaseAbilities ability)
        {
            if (ability.IsMultiple)
            {
                BaseAbilities[] prefabAbilities = new BaseAbilities[ability.AlternativeCountAbilities];
                for (int i = 0; i < ability.AlternativeCountAbilities; i++)
                {
                    prefabAbilities[i] = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability, _autoExpand);
                    PlaySound?.Invoke(prefabAbilities[i].SoundEnum);
                    prefabAbilities[i].SetDie += ReturnObjectToPool;

                    activeAbilities.Add(prefabAbilities[i]); // Add to the list
                }
            }
            else
            {
                BaseAbilities prefabAbility = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability, _autoExpand);
                PlaySound?.Invoke(prefabAbility.SoundEnum);
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