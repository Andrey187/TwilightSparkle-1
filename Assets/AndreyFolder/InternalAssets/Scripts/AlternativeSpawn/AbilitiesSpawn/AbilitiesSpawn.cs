using System;
using System.Collections;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilitiesSpawn : MonoCache
    {
        [SerializeField] private AttackSystem _attackSystem;
        private PoolObject<BaseAbilities> _abilityPool;
        private IObjectFactory _objectFactory;
        public event Action InitializationComplete;

        private void Start()
        {
            InitPool();
            _attackSystem.SetCreatePrefabAbility(CreatePrefabAbility);
            InitializationComplete?.Invoke();
        }

        protected override void OnDisabled()
        {
            _attackSystem.NewInitializationComplete -= CreateCloneAbility;
        }

        private void InitPool()
        {
            _attackSystem.NewInitializationComplete += CreateCloneAbility;
            foreach (var ability in _attackSystem.AttackScriptsList)
            {
                _objectFactory = new ObjectsFactory(ability.GetComponent<BaseAbilities>().transform);
                BaseAbilities baseAbilities = _objectFactory.CreateObject(_attackSystem.StartAttackPoint.position).GetComponent<BaseAbilities>();
                PoolObject<BaseAbilities>.CreateInstance(baseAbilities, 0, gameObject.transform, baseAbilities.name + "_Ability");
                _abilityPool = PoolObject<BaseAbilities>.Instance;
                CreateCloneAbility(baseAbilities);
            }
        }

        private void CreateCloneAbility(BaseAbilities ability)
        {
            BaseAbilities prefabAbility = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);
            prefabAbility.gameObject.SetActive(false);
            _attackSystem.InstantiateCloneAbilities.Add(prefabAbility);
        }

        private void CreatePrefabAbility(BaseAbilities ability, Vector3 targetPoint)
        {
            BaseAbilities prefabAbility = _abilityPool.GetObjects(_attackSystem.StartAttackPoint.position, ability);
            _attackSystem.InstantiateCloneAbilities.Add(prefabAbility);
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
            StartCoroutine(Duration(prefabAbility));
        }

        private IEnumerator Duration(BaseAbilities expiredAbility)
        {
            while (true)
            {
                if (expiredAbility.LifeTime <= 0)
                {
                    ReturnObjectToPool(expiredAbility);
                    yield break; // Exit the coroutine
                }
                yield return null;
            }
        }

        private void ReturnObjectToPool(BaseAbilities expiredAbility)
        {
            _abilityPool.ReturnObject(expiredAbility);
            expiredAbility.SetDie -= ReturnObjectToPool;
        }
    }
}