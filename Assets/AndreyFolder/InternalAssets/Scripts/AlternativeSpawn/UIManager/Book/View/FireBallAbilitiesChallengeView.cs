public class FireBallAbilitiesChallengeView : BaseChallengeView
{
    protected override void Awake()
    {
        base.Awake();
        _nameText.SetText("Collect FireBall Abilities");
        _rewardDescription.SetText("Reward + " + _reward[_statType] + " Massive FireBall");
        _maxCountValueText.SetText(_maxCount[_statType].ToString());
    }
}
