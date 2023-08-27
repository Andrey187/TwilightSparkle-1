using UnityEngine.UI;
using Zenject;

public class GainExpMyltiply : MonoCache
{
    [Inject] private ILevelUpSystem _levelUpSystem;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Trigger the AddAttackScript method of the AttackScript class
        if(_levelUpSystem.GainExpMyltiply == 1)
        {
            _levelUpSystem.GainExpMyltiply *= 20;
        }
        else { _levelUpSystem.GainExpMyltiply += 20; }
    }
}
