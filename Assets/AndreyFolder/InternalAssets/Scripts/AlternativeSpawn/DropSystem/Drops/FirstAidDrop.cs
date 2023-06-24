using UnityEngine;

public class FirstAidDrop : BaseDrop
{
    [SerializeField] private int _restorHp = 50;
    private Camera _camera;
    private Canvas _canvas;
    protected override void Start()
    {
        base.Start();
        _canvas = ChildrenGet<Canvas>();
        _camera = Camera.main;
        _canvas.worldCamera = _camera;
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
