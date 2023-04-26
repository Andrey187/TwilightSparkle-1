using System.ComponentModel;

public class HealthBarViewModel : INotifyPropertyChanged
{
    private HealthBarModel _model;

    public HealthBarViewModel(HealthBarModel model)
    {
        _model = model;
        _model.PropertyChanged += HandlePropertyChanged;
    }

    public int CurrentHealth
    {
        get { return _model.CurrentHealth; }
        set
        {
            _model.CurrentHealth = value;
            OnPropertyChanged(nameof(CurrentHealth));
        }
    }

    public int MaxHealth
    {
        get { return _model.MaxHealth; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HealthBarModel.CurrentHealth))
        {
            OnPropertyChanged(nameof(CurrentHealth));
        }
        else if (e.PropertyName == nameof(HealthBarModel.MaxHealth))
        {
            OnPropertyChanged(nameof(MaxHealth));
            OnPropertyChanged(nameof(CurrentHealth));
        }
    }
}


//private int _currentHealth;
//private int _maxHealth;

//public int CurrentHealth
//{
//    get { return _currentHealth; }
//    set
//    {
//        _currentHealth = value;
//        OnPropertyChanged(nameof(CurrentHealth));
//    }
//}

//public int MaxHealth
//{
//    get { return _maxHealth; }
//    set
//    {
//        _maxHealth = value;
//        OnPropertyChanged(nameof(MaxHealth));
//    }
//}

//_healthBarModel.OnTakeDamage += (enemy, amount) => OnPropertyChanged(nameof(CurrentHealth));
//_healthBarModel.OnHeal += (amount) => OnPropertyChanged(nameof(CurrentHealth));
