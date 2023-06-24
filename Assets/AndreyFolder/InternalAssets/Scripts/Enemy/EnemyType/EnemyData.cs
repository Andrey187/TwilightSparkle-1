using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyType", menuName = "Enemy/EnemyType")]
public class EnemyData : ScriptableObject, IResetOnExitPlay
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private int _gainExp;
    [SerializeField] private int _startingMaxHealth = 0;
    public ObjectType _type;

    public enum ObjectType
    {
        Type1,
        Type2,
        Type3,
        Boss
    }

    private void OnValidate()
    {
        // Assign the name of the ScriptableObject to _enemyName
        _enemyName = name;
    }

    public void SetCurrentHealthToMax()
    {
        _currentHealth = _maxHealth;
    }

    public void ResetOnExitPlay()
    {
        _maxHealth = _startingMaxHealth;
        SetCurrentHealthToMax();
    }

    public string EnemyName { get { return _enemyName; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int Damage { get { return _damage; } }
    public float Speed { get { return _speed; } }
    public int GainExp { get { return _gainExp; } set { _gainExp = value; } }
}