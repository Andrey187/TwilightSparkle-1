using UnityEngine;
using UnityEngine.UI;

public class GainExpMyltiply : MonoCache
{
    [SerializeField] private LevelUpSystem _levelUpSystem;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _levelUpSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUpSystem>();
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
