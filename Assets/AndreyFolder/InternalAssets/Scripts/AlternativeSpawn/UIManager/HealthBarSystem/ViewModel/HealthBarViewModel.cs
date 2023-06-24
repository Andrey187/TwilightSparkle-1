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
        }
    }

    public int MaxHealth
    {
        get { return _model.MaxHealth; }
        set
        {
            _model.MaxHealth = value;
        }
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
        if (e.PropertyName == nameof(HealthBarModel.MaxHealth))
        {
            OnPropertyChanged(nameof(MaxHealth));
            OnPropertyChanged(nameof(CurrentHealth));
        }
    }
}