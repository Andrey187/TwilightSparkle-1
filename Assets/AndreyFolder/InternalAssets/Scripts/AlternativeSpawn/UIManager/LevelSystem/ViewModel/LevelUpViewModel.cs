using System.ComponentModel;

public class LevelUpViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly LevelUpSystem _levelUpSystem;
    private readonly TalentPointsBinding _talentPointsBinding;

    public int CurrentLevel => _levelUpSystem.CurrentLevel;

    public int CurrentExp => _levelUpSystem.CurrentExp;

    public int NextLevelExp => _levelUpSystem.NextLevelExp;

    public int TalentPoints
    {
        get { return _levelUpSystem.TalentPoint; }
        set
        {
            _levelUpSystem.TalentPoint = value;
            OnPropertyChanged(nameof(TalentPoints));
        }
    }

    public LevelUpViewModel(LevelUpSystem levelUpSystem)
    {
        _levelUpSystem = levelUpSystem;
        _talentPointsBinding = TalentPointsBinding.Instance;
        _talentPointsBinding.RegisterLevelUpViewModel(this);
        _levelUpSystem.PropertyChanged += HandlePropertyChanged;
    }

    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_levelUpSystem.CurrentLevel):
                OnPropertyChanged(nameof(CurrentLevel));
                break;
            case nameof(_levelUpSystem.CurrentExp):
                OnPropertyChanged(nameof(CurrentExp));
                break;
            case nameof(_levelUpSystem.NextLevelExp):
                OnPropertyChanged(nameof(NextLevelExp));
                break;
            case nameof(_levelUpSystem.TalentPoint):
                OnPropertyChanged(nameof(TalentPoints));
                break;
        }
    }
}
