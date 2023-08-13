public class HpTalentView : BaseTalentView
{
    protected override void Start()
    {
        base.Start();
        _nameText.SetText("Increase Hp");
        _buttonText.SetText("+ " + _buttonTextValue.ToString()); 
        _maxTalentPointsText.SetText(_maxTalent[_statType].ToString());
    }
}
