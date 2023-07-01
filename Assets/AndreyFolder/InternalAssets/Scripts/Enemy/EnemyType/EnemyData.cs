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
    public ObjectType _type;

    [Header("Default Settings")]
    [SerializeField] private int _defaultMaxHealth = 0;
    [SerializeField] private int _defaultDamage = 0;
    [SerializeField] private float _defaultSpeed = 0;
    [SerializeField] private int _defaultGainExp = 0;

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
        _maxHealth = _defaultMaxHealth;
        _damage = _defaultDamage;
        _speed = _defaultSpeed;
        _gainExp = _defaultGainExp;
        SetCurrentHealthToMax();
    }

    public string EnemyName { get { return _enemyName; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public float Speed { get { return _speed; } }
    public int GainExp { get { return _gainExp; } set { _gainExp = value; } }
}