using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : MonoBehaviour
{
    [SerializeField] private LevelUpSystem _levelUpSystem;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _currentExpText;
    [SerializeField] private TextMeshProUGUI _nextLevelExpText;
    [SerializeField] private TextMeshProUGUI _talentPointsText;
    private LevelUpViewModel _viewModel;

    private void Start()
    {
        var levelUpSystem = _levelUpSystem;
        _viewModel = new LevelUpViewModel(levelUpSystem);

        _currentExpText.SetText(_viewModel.CurrentExp.ToString());
        _nextLevelExpText.SetText( _viewModel.NextLevelExp.ToString());
        _levelNumberText.SetText("Level: " + _viewModel.CurrentLevel.ToString());
        _talentPointsText.SetText("Talent Points: " + _viewModel.TalentPoints.ToString());
        UpdateExpSlider(_viewModel.NextLevelExp);

        _viewModel.PropertyChanged += HandlePropertyChanged;
    }

    private void UpdateExpSlider(int expToNextLevel)
    {
        _expSlider.maxValue = expToNextLevel;
        _expSlider.value = _viewModel.CurrentExp;
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_viewModel.CurrentLevel):
                _levelNumberText.SetText("Level: " + _viewModel.CurrentLevel.ToString());
                break;
            case nameof(_viewModel.CurrentExp):
                _currentExpText.SetText(_viewModel.CurrentExp.ToString());
                UpdateExpSlider(_viewModel.NextLevelExp);
                break;
            case nameof(_viewModel.NextLevelExp):
                _nextLevelExpText.SetText(_viewModel.NextLevelExp.ToString());
                break;
            case nameof(_viewModel.TalentPoints):
                _talentPointsText.SetText("Talent Points: " + _viewModel.TalentPoints.ToString());
                break;
        }
    }
}



