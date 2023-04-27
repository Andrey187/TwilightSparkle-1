using System.ComponentModel;

public class LevelUpViewModel : INotifyPropertyChanged
{
    private LevelUpSystem _levelUpSystem;
    public event PropertyChangedEventHandler PropertyChanged;

    public int CurrentLevel
    {
        get { return _levelUpSystem.CurrentLevel; }
    }

    public int CurrentExp
    {
        get { return _levelUpSystem.CurrentExp; }
    }

    public int NextLevelExp
    {
        get { return _levelUpSystem.NextLevelExp; }
    }

    public int TalantPoints
    {
        get { return _levelUpSystem.TalantPoint; }
    }

    public LevelUpViewModel(LevelUpSystem levelUpSystem)
    {
        _levelUpSystem = levelUpSystem;
        _levelUpSystem.PropertyChanged += HandlePropertyChanged;
    }

    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_levelUpSystem.CurrentLevel))
        {
            OnPropertyChanged(nameof(CurrentLevel));
        }
        if (e.PropertyName == nameof(_levelUpSystem.CurrentExp))
        {
            OnPropertyChanged(nameof(CurrentExp));
        }
        if(e.PropertyName == nameof(_levelUpSystem.NextLevelExp))
        {
            OnPropertyChanged(nameof(NextLevelExp));
        }
        if(e.PropertyName == nameof(_levelUpSystem.TalantPoint))
        {
            OnPropertyChanged(nameof(TalantPoints));
        }
    }
}
