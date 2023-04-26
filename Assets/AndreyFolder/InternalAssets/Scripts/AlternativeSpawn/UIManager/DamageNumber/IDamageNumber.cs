using UnityEngine;

public interface IDamageNumber
{
    public float LifeTime { get; set; }

    public Vector3 Direction { get; set; }

    public abstract void SetNumberDirection();

    public abstract void NumberReset();
}
