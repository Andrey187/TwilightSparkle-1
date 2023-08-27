using System.ComponentModel;
using UnityEngine;

public class HealthBarModel : MonoBehaviour,  IHealthBarModelParameters, INotifyPropertyChanged
{
    private int _currentHealth;
    private int _maxHealth;

    public event PropertyChangedEventHandler PropertyChanged;

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnPropertyChanged(nameof(CurrentHealth));
        }
    }

    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = Mathf.Clamp(value, 0, int.MaxValue);
            if (_maxHealth < _currentHealth)
            {
                _currentHealth = _maxHealth;
                OnPropertyChanged(nameof(CurrentHealth));
            }
            OnPropertyChanged(nameof(MaxHealth));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
