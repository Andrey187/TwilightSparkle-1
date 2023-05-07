
public class HpTalentView : BaseTalentView
{
    protected override void Start()
    {
        base.Start();
        _nameText.text = "Increase Hp";
        _buttonText.text = "+ " + _buttonTextValue.ToString(); 
        _maxTalentPointsText.text = _maxTalentsPointCount.ToString();
    }
}
