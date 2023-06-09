using UnityEngine;

public class FirstAidDrop : BaseDrop
{
    [SerializeField] private int _restorHp = 50;

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            if(_playerStats.CurrentHealth < _playerStats.MaxHealth)
            {
                _playerStats.CurrentHealth += _restorHp;
                _playerStats.CurrentHealth = Mathf.Min(_playerStats.CurrentHealth, _playerStats.MaxHealth);
                PoolObject<BaseDrop>.Instance.ReturnObject(this);
            }
        }
    }
}
