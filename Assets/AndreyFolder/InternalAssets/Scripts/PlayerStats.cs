using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 0;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _talentPoints = 0;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _currentExp = 0;

    private void Start()
    {
        SetCurrentHealthToMax();
    }

    public void SetCurrentHealthToMax()
    {
        _currentHealth = _maxHealth;
    }

    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    public int MaxHealth { get { return _maxHealth; }  set { _maxHealth = value; }}

    public int TalentPoints { get { return _talentPoints; }set { _talentPoints = value; } }

    public int CurrentLevel { get { return _currentLevel; } set { _currentLevel = value; } }

    public int CurrentExp { get { return _currentExp; } set { _currentExp = value; } }

}
