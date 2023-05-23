using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>();
        [SerializeField] private Transform _startAttackPoint;
        [SerializeField] private Transform _endPoint;
        private event Action<BaseAbilities> _createPrefabAbility;

        public List<BaseAbilities> AttackScriptsList { get => _attackAbilities; set => _attackAbilities = value; }
        private void Awake()
        {
            if (_startAttackPoint == null) { _startAttackPoint = transform.Find("StartAttackPoint"); }
            if (_endPoint == null) { _endPoint = transform.Find("EndPoint"); }

            foreach (var ability in _attackAbilities)
            {
                SetPositions(ability);
            }
        }

        private void Start()
        {
            // Spawn the first ability
            if (_attackAbilities.Count > 0)
            {
                SetPositions(_attackAbilities[0]);
                StartCoroutine(SpawnAbilitiesCoroutine(_attackAbilities[0]));
            }
        }

        private void SetPositions(BaseAbilities ability)
        {
            ability.StartPoint = _startAttackPoint;
            ability.EndPoint = _endPoint;
        }

        private void SetUpInvokeRepeatingForAbility(BaseAbilities ability)
        {
            SetPositions(ability);
            StartCoroutine(SpawnAbilitiesCoroutine(ability));
        }
        private IEnumerator SpawnAbilitiesCoroutine(BaseAbilities ability)
        {
            float lastSpawnTime = Time.time - ability.FireInterval;
            while (true)
            {
                float timeSinceLastSpawn = Time.time - lastSpawnTime;
                if (timeSinceLastSpawn >= ability.FireInterval)
                {
                    InvokeCreatePrefabAbility(ability);
                    lastSpawnTime = Time.time;
                }
                yield return null;
            }
        }

        public void SetCreatePrefabAbility(Action<BaseAbilities> createPrefabAbility)
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void InvokeCreatePrefabAbility(BaseAbilities ability)
        {
            _createPrefabAbility?.Invoke(ability);
        }

        public void AddAttackScript(BaseAbilities ability)
        {
            _attackAbilities.Add(ability);
            SetUpInvokeRepeatingForAbility(_attackAbilities[_attackAbilities.Count - 1]);
        }
    }
}