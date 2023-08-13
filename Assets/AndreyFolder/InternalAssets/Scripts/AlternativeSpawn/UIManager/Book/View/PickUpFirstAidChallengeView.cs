public class PickUpFirstAidChallengeView : BaseChallengeView
{
    protected override void Awake()
    {
        base.Awake();
        _nameText.SetText("Pick Up First Aid boxes");
        _rewardDescription.SetText("Reward + " + _reward[_statType] + "% chance drop");
        _maxCountValueText.SetText(_maxCount[_statType].ToString());
    }
}
