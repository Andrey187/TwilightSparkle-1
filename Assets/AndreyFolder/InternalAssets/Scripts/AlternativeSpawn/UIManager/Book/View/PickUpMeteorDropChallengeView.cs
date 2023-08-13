public class PickUpMeteorDropChallengeView : BaseChallengeView
{
    protected override void Awake()
    {
        base.Awake();
        _nameText.SetText("Pick Up Meteor Sphere");
        _rewardDescription.SetText("Reward + " + _reward[_statType] + "% chance drop");
        _maxCountValueText.SetText(_maxCount[_statType].ToString());
    }
}
