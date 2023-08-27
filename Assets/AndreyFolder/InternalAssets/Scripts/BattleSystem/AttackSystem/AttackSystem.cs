using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AttackSystem : MonoBehaviour, IAttackSystem
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>(); //������ ������������
        [SerializeField] private Transform _startAttackPoint; //��������� �������
        [SerializeField] private Transform _endPoint; // �������� �������

        private event Action<BaseAbilities> _createPrefabAbility; //������� ��� �������� ������ �� AbilitiesSpawn
        private AbilitiesSpawn _abilitiesSpawn;

        public List<BaseAbilities> AttackAbilitiesList { get => _attackAbilities; set => _attackAbilities = value; }
        public Transform StartAttackPoint { get => _startAttackPoint; set => _startAttackPoint = value; }
        public Transform EndAttackPoint { get => _endPoint; set => _endPoint = value; }

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
                InvokeCreatePrefabAbility(ability);
                yield return new WaitForSeconds(ability.FireInterval);
            }
        }

        public void SetCreatePrefabAbility(Action<BaseAbilities> createPrefabAbility) //1. ���������� �� ������ � AbilitiesSpawn � ���������� ����� CreatePrefabAbility
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void InvokeCreatePrefabAbility(BaseAbilities ability) //6.���������� ����� CreatePrefabAbility �� AbilitiesSpawn � ���������� ability � targetPoint
        {
            _createPrefabAbility?.Invoke(ability);
        }

        public void AddAttack(BaseAbilities ability) //����� ���������� �� ������� �� ������, ��������� ����� ����������� � ������, �������� ����� ������
        {
            _attackAbilities.Add(ability);
           
            SetUpInvokeRepeatingForAbility(ability);
        }
    }
}