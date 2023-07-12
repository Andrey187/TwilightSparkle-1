using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AbilitySystem
{
    public class AttackSystem : MonoCache
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>(); //������ ������������
        [SerializeField] private Transform _startAttackPoint; //��������� �������
        [SerializeField] private Transform _endPoint; // �������� �������

        private event Action<BaseAbilities, Vector3> _createPrefabAbility; //������� ��� �������� ������ �� AbilitiesSpawn
        private AbilitiesSpawn _abilitiesSpawn;

        public Vector3 _nearestEnemyPosition;
        public List<BaseAbilities> AttackAbilitiesList { get => _attackAbilities; set => _attackAbilities = value; }
        public Transform StartAttackPoint { get => _startAttackPoint; set => _startAttackPoint = value; }
        public Transform EndAttackPoint { get => _endPoint; set => _endPoint = value; }

        private void Awake()
        {
            if (_startAttackPoint == null) { _startAttackPoint = transform.Find("StartAttackPoint"); }
            if (_endPoint == null) { _endPoint = transform.Find("EndPoint"); }
        }

        private void Start()
        {
            _abilitiesSpawn = FindObjectOfType<AbilitiesSpawn>();
            _abilitiesSpawn.InitializationComplete += FirstAbilitySpawnStart; 
            SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
        }

        private void UnsubscribeEvents()
        {
            _abilitiesSpawn.InitializationComplete -= FirstAbilitySpawnStart;
        }

        public Vector3 FindNearestEnemyInArea(float areaRadius) //5. ����� ������ ���������� ����������. ����� ��� ����������
        {
            Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, areaRadius, LayerMask.GetMask("Enemy"));

            if (detectedEnemies.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, detectedEnemies.Length);
                return detectedEnemies[randomIndex].transform.position;
            }

            return Vector3.zero;
        }
        
        private void SetUpInvokeRepeatingForAbility(BaseAbilities ability) //3.
        {
            StartCoroutine(SpawnAbilitiesCoroutine(ability));
        }

        private void FirstAbilitySpawnStart() // 2. ��� ������ ����������� �� ������ InstantiateCloneAbilities ����������� ��������, � ������� ���������� �����������
        {
            SetUpInvokeRepeatingForAbility(AttackAbilitiesList[0]);
        }

        private IEnumerator SpawnAbilitiesCoroutine(BaseAbilities ability) //4.
        {
            while (true)
            {
                _nearestEnemyPosition = FindNearestEnemyInArea(ability.AreaRadius);
                InvokeCreatePrefabAbility(ability, _nearestEnemyPosition);
                yield return new WaitForSeconds(ability.FireInterval);
            }
        }

        public void SetCreatePrefabAbility(Action<BaseAbilities, Vector3> createPrefabAbility) //1. ���������� �� ������ � AbilitiesSpawn � ���������� ����� CreatePrefabAbility
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void InvokeCreatePrefabAbility(BaseAbilities ability, Vector3 targetPoint) //6.���������� ����� CreatePrefabAbility �� AbilitiesSpawn � ���������� ability � targetPoint
        {
            _createPrefabAbility?.Invoke(ability, targetPoint);
        }

        public void AddAttackScript(BaseAbilities ability) //����� ���������� �� ������� �� ������, ��������� ����� ����������� � ������, �������� ����� ������
        {
            _attackAbilities.Add(ability);
            SetUpInvokeRepeatingForAbility(ability);
        }
    }
}