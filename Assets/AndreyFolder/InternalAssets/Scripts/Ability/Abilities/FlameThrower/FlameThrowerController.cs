using UnityEngine;

public class FlameThrowerController : MonoCache
{
    [SerializeField] private GameObject _flameThrower;
    [SerializeField] private float _flameThrowerAbilitydDuration = 5f; // Change this value to set the shield duration in seconds
    private float _currentflameThrowerTime = 0f;
    private bool _isFlameThrowerActive = false;

    public bool IsFlameThrowerActive
    {
        get { return _isFlameThrowerActive; }
    }

    protected override void Run()
    {
        if (_isFlameThrowerActive)
        {
            _currentflameThrowerTime -= Time.deltaTime;
            if (_currentflameThrowerTime <= 0f)
            {
                DeactivateFlameThrower();
            }
        }
    }

    public void ActivateFlameThrower()
    {
        _isFlameThrowerActive = true;
        _currentflameThrowerTime = _flameThrowerAbilitydDuration;
        _flameThrower.SetActive(true);
    }

    private void DeactivateFlameThrower()
    {
        _isFlameThrowerActive = false;
        _currentflameThrowerTime = 0f;
        _flameThrower.SetActive(false);
    }
}
