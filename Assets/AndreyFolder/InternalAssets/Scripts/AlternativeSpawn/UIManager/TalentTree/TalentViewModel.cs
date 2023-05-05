using System;
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

    public TalentViewModel(TalentSystem talent)
    {
        _talentSystem = talent;
        _talentPointsBinding = TalentPointsBinding.Instance;
        _talentPointsBinding.RegisterTalentViewModel(this);
        _talentSystem.PropertyChanged += HandlePropertyChanged;
    }

    public void OnButtonClick(int buttonValue)
    {
        _talentSystem.Upgrade(buttonValue,_pointsInvested);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_talentSystem.TalentPoints))
        {
            OnPropertyChanged(nameof(TalentPoints));
            _talentPointsBinding.OnTalentPointsChanged(TalentPoints);
        }
    }
}
