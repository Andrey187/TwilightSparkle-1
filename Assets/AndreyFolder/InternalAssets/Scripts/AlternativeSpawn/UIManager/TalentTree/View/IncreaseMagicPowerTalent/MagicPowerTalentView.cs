public class MagicPowerTalentView : BaseTalentView
{
    protected override void Start()
    {
        base.Start();
        _nameText.SetText("Increase Power");
        _buttonText.SetText("+ " + _buttonTextValue.ToString() + "%");
        _maxTalentPointsText.SetText(_maxTalent[_statType].ToString());
    }
}
