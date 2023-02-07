using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string description;

    [SerializeField] private Material _enemyMaterial;

    [SerializeField] private int _enemySpeed;
}

