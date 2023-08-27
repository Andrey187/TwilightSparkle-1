using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AttackSystem : MonoBehaviour, IAttackSystem
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>(); //список способностей
        [SerializeField] private Transform _startAttackPoint; //начальная позиция
        [SerializeField] private Transform _endPoint; // конечная позиция

        private event Action<BaseAbilities> _createPrefabAbility; //событие для передачи метода из AbilitiesSpawn
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

        private void FirstAbilitySpawnStart() // 2. для каждой способности из списка InstantiateCloneAbilities запускается корутина, в которую передаются способности
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

        public void SetCreatePrefabAbility(Action<BaseAbilities> createPrefabAbility) //1. вызывается на старте в AbilitiesSpawn и передается метод CreatePrefabAbility
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void InvokeCreatePrefabAbility(BaseAbilities ability) //6.вызывается метод CreatePrefabAbility из AbilitiesSpawn и передается ability и targetPoint
        {
            _createPrefabAbility?.Invoke(ability);
        }

        public void AddAttack(BaseAbilities ability) //Метод вызывается по нажатию на кнопку, добавляет новую способность в список, вызывает метод спавна
        {
            _attackAbilities.Add(ability);
           
            SetUpInvokeRepeatingForAbility(ability);
        }
    }
}