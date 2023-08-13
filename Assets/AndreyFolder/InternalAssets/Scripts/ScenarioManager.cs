using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoCache
{
    [SerializeField] private int _countLevelForDecreaseDuration;
    [SerializeField] private float _durationTime;
    [SerializeField] private int _countLevelForIncreaseEnemyHp;
    [SerializeField] private int _increaseMaxHpEnemy;
    [SerializeField] private List<TalentSO> _talents;
    private PlayerEventManager _playerEvent;
    private WaveSpawner _waveSpawner;

    private void Start()
    {
        _playerEvent = PlayerEventManager.Instance;
        _waveSpawner = FindObjectOfType<WaveSpawner>();

        if (_playerEvent != null)
        {
            _playerEvent.PlayerLevelUp += UpdateScenario;
        }
        else
        {
            Debug.Log("null");
        }
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        _playerEvent.PlayerLevelUp -= UpdateScenario;
    }

    private void UpdateScenario(int newLevel)
    {
        // Decrease wave duration every _countLevelForDecreaseDuration character levels
        if (newLevel % _countLevelForDecreaseDuration == 0)
        {
            float decreaseAmount = _durationTime;
            // Calculate the updated values based on the player's level
            for (int i = 0; i < _waveSpawner.Waves.Length; i++)
            {
                float waveDuration = _waveSpawner.Waves[i].WaveDuration;
                float newWaveDuration = waveDuration - decreaseAmount;// Decrease by _durationTime seconds

                float minWaveDuration = _waveSpawner.Waves[i].BaseDuration / 3f;
                newWaveDuration = Mathf.Max(newWaveDuration, minWaveDuration);

                // Apply the updated values to the wave spawner and enemy prefab
                _waveSpawner.Waves[i].WaveDuration = newWaveDuration;
            }
        }

        if (newLevel % _countLevelForIncreaseEnemyHp == 0)
        {
            foreach (EnemyData enemyData in DataLoader.Instance._enemyDataBase.EnemyDataList)
            {
                enemyData.MaxHealth += enemyData.MaxHealth/3;
                enemyData.GainExp += enemyData.GainExp / 4;
                enemyData.Speed += 0.1f;
            }
        }

        if(newLevel % 10 == 0) 
        {
            foreach (EnemyData enemyData in DataLoader.Instance._enemyDataBase.EnemyDataList)
            {
                enemyData.MaxHealth += 500;
                enemyData.Damage += 5;
            }
        }

        if (newLevel % 15 == 0)
        {
            foreach (var talent in _talents)
            {
                talent.ChangeMaxTalentPoints(2);
            }
        }

        if (newLevel % 4 == 0)
        {
            _waveSpawner.BossSpawn(_waveSpawner.Waves[0]);
        }
    }
}
