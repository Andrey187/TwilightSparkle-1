using System;
using UnityEngine;

public class PlayerStats : MonoCache
{
    [SerializeField] private int _currentHealth = 0;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _talentPoints = 0;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _speed = 6;
    private HealthBarController _healthBarController;

    private void Start()
    {
        _healthBarController = Get<HealthBarController>();
        SetCurrentHealthToMax();
    }

    public void SetCurrentHealthToMax()
    {
        _currentHealth = _maxHealth;
        _healthBarController.Initialize(_maxHealth);
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Max(value, 0);

            if (_currentHealth <= 0)
            {
                // Handle player death or other actions
                // ...
            }
            else
            {
                _healthBarController.SetCurrentHealth(_currentHealth);
            }
        }
    }

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public int TalentPoints { get => _talentPoints; set => _talentPoints = value; }

    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    public int CurrentExp { get => _currentExp; set => _currentExp = value; }

    public int Speed { get => _speed; 
        set
        {
            _speed = value;
            OnSpeedChanged(); // Invoke the event when the speed changes
        }
    }

    public event Action SpeedChanged;

    private void Die()
    {
        
    }

    private void OnSpeedChanged()
    {
        SpeedChanged?.Invoke();
    }
}
