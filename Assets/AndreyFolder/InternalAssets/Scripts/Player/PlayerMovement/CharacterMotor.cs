using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private int _runSpeed = 0;
    [SerializeField] private PlayerStats _playerStats;
    private Rigidbody _rigidbody;

    public int RunSpeed { get => _runSpeed; set => _runSpeed = value; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _runSpeed = _playerStats.Speed;
        _playerStats.SpeedChanged += UpdateRunSpeed;
    }

    private void UpdateRunSpeed()
    {
        _runSpeed = _playerStats.Speed;
    }

    public void Move(Vector3 direction, int speed)
    {
        Vector3 movement = direction * speed;
        _rigidbody.velocity = new Vector3(movement.x, 0f, movement.z);
    }

    public void Rotate(Vector3 direction)
    {
        // Rotate the character to face the movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(targetRotation);
        }
    }
}
