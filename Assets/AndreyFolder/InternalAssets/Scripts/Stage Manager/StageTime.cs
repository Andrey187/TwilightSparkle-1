using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTime : MonoCache
{
    /// <summary>
    /// in the future may be timer ui
    /// </summary>
    [SerializeField] public float Timer; //time after which the wave will start

    protected override void Run()
    {
        Timer += Time.deltaTime;
    }
}
