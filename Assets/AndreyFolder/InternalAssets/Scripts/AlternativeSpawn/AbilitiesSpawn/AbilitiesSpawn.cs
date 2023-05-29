using System.Collections;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilitiesSpawn : MonoBehaviour
    {
        [SerializeField] private AttackSystem _attackSystem;
        private PoolObject<BaseAbilities> _abilityPool;
        private IObjectFactory _objectFactory;

        private void Start()
        {
            InitPool();
            _attackSystem.SetCreatePrefabAbility(CreatePrefabAbility);
        }

        private void InitPool()
        {
            foreach (var ability in _attackSystem.AttackScriptsList)
            {
                _objectFactory = new ObjectsFactory(ability.GetComponent<BaseAbilities>().transform);
                BaseAbilities baseAbilities = _objectFactory.CreateObject(ability.StartPoint.position).GetComponent<BaseAbilities>();

                PoolObject<BaseAbilities>.CreateInstance(baseAbilities, 0, gameObject.transform, baseAbilities.name + "_Ability");
                _abilityPool = PoolObject<BaseAbilities>.Instance;
            }
        }

        private void CreatePrefabAbility(BaseAbilities ability, Vector3 targetPoint)
        {
            BaseAbilities prefabAbility = _abilityPool.GetObjects(ability.StartPoint.position, ability);
            if (ability.HasTargetPoint)
            {
                prefabAbility.TargetPoint = targetPoint;
                prefabAbility.MoveWithPhysics();
            }
            else
            {
                prefabAbility.MoveWithPhysics();
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