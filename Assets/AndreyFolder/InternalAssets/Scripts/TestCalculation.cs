using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalculation : MonoBehaviour
{
    private AbstractCalculation _abstractCalculation;

    [SerializeField] private float _spawnRadius = 1.5f;
    [SerializeField] private float _colliderHitRadius = 0.3f;
    [SerializeField] private float _groupSpawn—ircleRadius = 20f;
    private Transform _player;

    private void Start()
    {
        //_abstractCalculation = new AbstractCalculation(_spawnRadius, _colliderHitRadius, _groupSpawn—ircleRadius, _player);
    }
}
