using UnityEngine;

public class MagicShield : MonoBehaviour
{
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private float _shieldDuration = 5f; // Change this value to set the shield duration in seconds
    private float _currentShieldTime = 0f;
    private bool _isShieldActive = false;

    public bool IsShieldActive
    {
        get { return _isShieldActive; }
    }

    private void Update()
    {
        if (_isShieldActive)
        {
            _currentShieldTime -= Time.deltaTime;
            if (_currentShieldTime <= 0f)
            {
                DeactivateShield();
            }
        }
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _currentShieldTime = _shieldDuration;
        _shieldPrefab.SetActive(true);
    }

    public void DeactivateShield()
    {
        _isShieldActive = false;
        _currentShieldTime = 0f;
        _shieldPrefab.SetActive(false);
    }
}
