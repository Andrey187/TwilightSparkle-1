public class EnemiesKillChallengeView : BaseChallengeView
{
    protected override void Awake()
    {
        base.Awake();
        _nameText.SetText("Kill the given amount of enemies");
        _rewardDescription.SetText("Reward + " + _reward[_statType] + " Exp");
        _maxCountValueText.SetText(_maxCount[_statType].ToString());
    }
}
