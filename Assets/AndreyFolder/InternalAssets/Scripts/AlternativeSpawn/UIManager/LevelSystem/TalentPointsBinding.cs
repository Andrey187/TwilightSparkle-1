public class TalentPointsBinding
{
    private TalentViewModel _talentViewModel;
    private LevelUpViewModel _levelUpViewModel;

    public static TalentPointsBinding Instance { get; } = new TalentPointsBinding();

    public void RegisterTalentViewModel(TalentViewModel viewModel)
    {
        _talentViewModel = viewModel;
    }

    public void RegisterLevelUpViewModel(LevelUpViewModel viewModel)
    {
        _levelUpViewModel = viewModel;
    }

    public void OnTalentPointsChanged(int talentPoints)
    {
        _levelUpViewModel.TalentPoints = talentPoints;
        _talentViewModel.TalentPoints = talentPoints;
    }
}

