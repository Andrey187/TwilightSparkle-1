using System.Collections.Generic;
using System.ComponentModel;

public class TalentViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly TalentSystem _talentSystem;
    private readonly TalentPointsBinding _talentPointsBinding;
    private int _pointsInvested = 1;
    private int _talentPoints;
    public int TalentPoints
    {
        get { return _talentPoints; }
        set
        {
            if (_talentPoints != value)
            {
                _talentPoints = value;
                OnPropertyChanged(nameof(TalentPoints));
            }
        }
    }

    public Dictionary<TalentStatType, int> MaxTalentPoints
    {
        get { return _talentSystem.MaxTalentPoints; }
        set { _talentSystem.MaxTalentPoints = value; }
    }

    public Dictionary<TalentStatType, int> CurrentTalentPointsValue
    {
        get { return _talentSystem.CurrentTalentPointsValue; }
        set { _talentSystem.CurrentTalentPointsValue = value; }
    }

    public TalentViewModel(TalentSystem talentSystem)
    {
        _talentSystem = talentSystem;
        _talentPointsBinding = TalentPointsBinding.Instance;
        _talentPointsBinding.RegisterTalentViewModel(this);
        _talentSystem.PropertyChanged += HandlePropertyChanged;
    }

    public void OnButtonClick(TalentStatType statType, int buttonValue)
    {
        _talentSystem.Upgrade(statType,buttonValue, _pointsInvested);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_talentSystem.TalentPoints):
                OnPropertyChanged(nameof(TalentPoints));
                _talentPointsBinding.OnTalentPointsChanged(TalentPoints);
                break;
            case nameof(_talentSystem.MaxTalentPoints):
                OnPropertyChanged(nameof(MaxTalentPoints));
                break;
            case nameof(_talentSystem.CurrentTalentPointsValue):
                OnPropertyChanged(nameof(CurrentTalentPointsValue));
                break;
        }
    }
}
