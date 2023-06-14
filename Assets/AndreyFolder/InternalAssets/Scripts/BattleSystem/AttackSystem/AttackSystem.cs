using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AbilitySystem
{
    public class AttackSystem : MonoCache
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>();
        [SerializeField] private Transform _startAttackPoint;
        [SerializeField] private Transform _endPoint;

        private event Action<BaseAbilities, Vector3> _createPrefabAbility;
        private AbilitiesSpawn _abilitiesSpawn;
        private HashSet<BaseAbilities> _instantiateCloneAbilities = new HashSet<BaseAbilities>();
        public Vector3 _nearestEnemyPosition;
        public event Action<BaseAbilities> NewInitializationComplete;
        public List<BaseAbilities> AttackScriptsList { get => _attackAbilities; set => _attackAbilities = value; }
        public HashSet<BaseAbilities> InstantiateCloneAbilities { get => _instantiateCloneAbilities; set => _instantiateCloneAbilities = value; }
        public Transform StartAttackPoint { get => _startAttackPoint; set => _startAttackPoint = value; }
        public Transform EndAttackPoint { get => _endPoint; set => _endPoint = value; }

        private void Awake()
        {
            if (_startAttackPoint == null) { _startAttackPoint = transform.Find("StartAttackPoint"); }
            if (_endPoint == null) { _endPoint = transform.Find("EndPoint"); }
        }

        private void Start()
        {
            // Spawn the first ability
            if (_attackAbilities.Count > 0)
            {
                _nearestEnemyPosition = FindNearestEnemyInArea(_attackAbilities[0].AreaRadius);
                InvokeCreatePrefabAbility(_attackAbilities[0], _nearestEnemyPosition);
            }
            _abilitiesSpawn = FindObjectOfType<AbilitiesSpawn>();
            _abilitiesSpawn.InitializationComplete += OnInitializationComplete;
        }

        protected override void OnDisabled()
        {
            _abilitiesSpawn.InitializationComplete -= OnInitializationComplete;
        }

        public Vector3 FindNearestEnemyInArea(float areaRadius)
        {
            Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, areaRadius, LayerMask.GetMask("Enemy"));

            if (detectedEnemies.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, detectedEnemies.Length);
                return detectedEnemies[randomIndex].transform.position;
            }

            return Vector3.zero;
        }
        
        private void SetUpInvokeRepeatingForAbility(BaseAbilities ability)
        {
            StartCoroutine(SpawnAbilitiesCoroutine(ability));
        }

        private void OnInitializationComplete()
        {
            foreach (var ability in InstantiateCloneAbilities)
            {
                SetUpInvokeRepeatingForAbility(ability);
            }
        }

        private IEnumerator SpawnAbilitiesCoroutine(BaseAbilities ability)
        {
            while (true)
            {
                _nearestEnemyPosition = FindNearestEnemyInArea(ability.AreaRadius);
                InvokeCreatePrefabAbility(ability, _nearestEnemyPosition);
                yield return new WaitForSeconds(ability.FireInterval);
            }
        }

        public void SetCreatePrefabAbility(Action<BaseAbilities, Vector3> createPrefabAbility)
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void InvokeCreatePrefabAbility(BaseAbilities ability, Vector3 targetPoint)
        {
            _createPrefabAbility?.Invoke(ability, targetPoint);
        }

        public void AddAttackScript(BaseAbilities ability)
        {
            _attackAbilities.Add(ability);
            NewInitializationComplete?.Invoke(ability);
            SetUpInvokeRepeatingForAbility(InstantiateCloneAbilities.LastOrDefault());
        }
    }
}