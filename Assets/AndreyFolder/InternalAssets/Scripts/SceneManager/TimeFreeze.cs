using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeFreeze : MonoCache
{
    private Rigidbody _rb;
    [SerializeField] private bool _isActive = false;

    private void Start()
    {
        _rb = Get<Rigidbody>();
    }

    protected override void Run()
    {
        if (_isActive)
        {
            _rb.isKinematic = true;
        }
        else { _rb.isKinematic = false; }
    }

    private void TimeStop()
    {
        _isActive = !_isActive;
    }
}
