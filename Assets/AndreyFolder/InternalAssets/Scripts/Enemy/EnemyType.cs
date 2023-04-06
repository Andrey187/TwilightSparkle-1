using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyType", menuName = "Enemy/EnemyType")]
public class EnemyType : ScriptableObject
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    private void OnValidate()
    {
        // Assign the name of the ScriptableObject to _enemyName
        _enemyName = name;
    }

    public void SetCurrentHealthToMax()
    {
        _currentHealth = _maxHealth;
    }

    public string EnemyName { get { return _enemyName; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int Damage { get { return _damage; } }
    public float Speed { get { return _speed; } }
    public Mesh Mesh { get { return _mesh; } }

    public Material Material { get { return _material; } }
}