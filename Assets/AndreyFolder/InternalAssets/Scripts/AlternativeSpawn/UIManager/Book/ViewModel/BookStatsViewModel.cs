using System.Collections.Generic;
using System.ComponentModel;

public class BookStatsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly BookStatsModel _bookStatsModel;

    public AutoClickButton AutoClickButton
    {
        get { return _bookStatsModel.AutoClickButton; }
        set { _bookStatsModel.AutoClickButton = value; }
    }

    public AttentionController AttentionController
    {
        get { return _bookStatsModel.AttentionController; }
        set { _bookStatsModel.AttentionController = value; }
    }

    public Dictionary<ChallengeType, int> RewardDescription
    {
        get { return _bookStatsModel.RewardDescription; }
        set { _bookStatsModel.RewardDescription = value; }
    }

    public Dictionary<ChallengeType, int> MaxCountValue
    {
        get { return _bookStatsModel.MaxCountValue; }
        set { _bookStatsModel.MaxCountValue = value; }
    }

    public Dictionary<ChallengeType, int> CurrentCountValue
    {
        get { return _bookStatsModel.CurrentCountValue; }
        set { _bookStatsModel.CurrentCountValue = value; }
    }

    public BookStatsViewModel(BookStatsModel bookStatsModel)
    {
        _bookStatsModel = bookStatsModel;
        _bookStatsModel.PropertyChanged += HandlePropertyChanged;
    }
    
    public void OnButtonClick(ChallengeType statType)
    {
        _bookStatsModel.Reward(statType);
    }

    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CurrentCountValue):
                OnPropertyChanged(nameof(CurrentCountValue));
                break;
            case nameof(MaxCountValue):
                OnPropertyChanged(nameof(MaxCountValue));
                break;
        }
    }
}