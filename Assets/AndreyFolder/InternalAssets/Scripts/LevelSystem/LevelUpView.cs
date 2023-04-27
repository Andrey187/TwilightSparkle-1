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
    [SerializeField] private TextMeshProUGUI _talantPoints;

    private LevelUpViewModel _viewModel;

    private void Start()
    {
        var levelUpSystem = _levelUpSystem;
        _viewModel = new LevelUpViewModel(levelUpSystem);

        _currentExpText.text = _viewModel.CurrentExp.ToString();
        _nextLevelExpText.text = _viewModel.NextLevelExp.ToString();
        _levelNumberText.text = "Level: " + _viewModel.CurrentLevel.ToString();
        _talantPoints.text = "Talant Points: " + _viewModel.TalantPoints.ToString();
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
        if (e.PropertyName == nameof(_viewModel.CurrentLevel))
        {
            _levelNumberText.text = "Level: " +_viewModel.CurrentLevel.ToString();
        }
        if (e.PropertyName == nameof(_viewModel.CurrentExp))
        {
            _currentExpText.text = _viewModel.CurrentExp.ToString();
            UpdateExpSlider(_viewModel.NextLevelExp);
        }
        if (e.PropertyName == nameof(_viewModel.NextLevelExp))
        {
            _nextLevelExpText.text = _viewModel.NextLevelExp.ToString();
        }
        if(e.PropertyName == nameof(_viewModel.TalantPoints))
        {
            _talantPoints.text = "Talant Points: " + _viewModel.TalantPoints.ToString();
        }
    }
}
