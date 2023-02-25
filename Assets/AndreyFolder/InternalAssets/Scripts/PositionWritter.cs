using UnityEngine;

public class PositionWritter : MonoCache
{
    [SerializeField] private Position _position;

    protected override void Run() => _position.Value = transform.position;
}
