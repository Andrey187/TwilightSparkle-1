using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace AbilitySystem
{
    public class AttackSystem : MonoBehaviour, IAttackSystem
    {
        [SerializeField] private List<BaseAbilities> _attackAbilities = new List<BaseAbilities>(); //список способностей
        [SerializeField] private Transform _startAttackPoint; //начальная позиция
        [SerializeField] private Transform _endPoint; // конечная позиция

        [Inject] private IGamePause _gamePause;
        private event Action<BaseAbilities> _createPrefabAbility; //событие для передачи метода из AbilitiesSpawn
        private AbilitiesSpawn _abilitiesSpawn;
        private CancellationTokenSource _cancellationTokenSource;

        public List<BaseAbilities> AttackAbilitiesList { get => _attackAbilities; set => _attackAbilities = value; }
        public Transform StartAttackPoint { get => _startAttackPoint; set => _startAttackPoint = value; }
        public Transform EndAttackPoint { get => _endPoint; set => _endPoint = value; }

        private void Start()
        {
            _abilitiesSpawn = FindObjectOfType<AbilitiesSpawn>();
            _cancellationTokenSource = new CancellationTokenSource();
            _abilitiesSpawn.InitializationComplete += FirstAbilitySpawnStart;
            SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
        }
        private void UnsubscribeEvents()
        {
            _cancellationTokenSource?.Cancel();
            _abilitiesSpawn.InitializationComplete -= FirstAbilitySpawnStart;
        }

        public void SetCreatePrefabAbility(Action<BaseAbilities> createPrefabAbility) //1. вызывается на старте в AbilitiesSpawn и передается метод CreatePrefabAbility
        {
            _createPrefabAbility = createPrefabAbility;
        }

        private void FirstAbilitySpawnStart() // 2. 
        {
            SetUpInvokeRepeatingForAbility(AttackAbilitiesList[0]);
        }

        private void SetUpInvokeRepeatingForAbility(BaseAbilities ability) //3.для каждой способности из списка запускается корутина, в которую передаются способности
        {
            _ = SpawnAbilitiesCoroutine(ability, _cancellationTokenSource.Token);
        }

        private async UniTask SpawnAbilitiesCoroutine(BaseAbilities ability, CancellationToken cancellationToken) //4.
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!_gamePause.IsPaused) // Проверяем, не установлена ли пауза
                {
                    InvokeCreatePrefabAbility(ability);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(ability.FireInterval), false, PlayerLoopTiming.Update, cancellationToken);
            }
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