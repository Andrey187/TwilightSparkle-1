public class SpeedTalentView : BaseTalentView
{
    protected override void Start()
    {
        base.Start();
        _nameText.SetText("Increase Speed");
        _buttonText.SetText("+ " + _buttonTextValue.ToString());
        _maxTalentPointsText.SetText(_maxTalentsPointCount.ToString());
    }
}
