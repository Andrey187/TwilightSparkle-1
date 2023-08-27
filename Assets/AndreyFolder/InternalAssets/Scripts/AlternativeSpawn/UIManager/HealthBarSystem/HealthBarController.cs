using UnityEngine;

public class HealthBarController : MonoCache
{
    [SerializeField] public HealthBarView _healthBarView;
    [SerializeField] private HealthBarModel _healthBarModel;
    private HealthBarViewModel _viewModel;

    private void Awake()
    {
        _healthBarModel = Get<HealthBarModel>();
    }

    public void Initialize(int maxHealth)
    {
        var model = _healthBarModel;
        model.MaxHealth = maxHealth;
        model.CurrentHealth = maxHealth;
        _viewModel = new HealthBarViewModel(model);
        _healthBarView.SetViewModel(_viewModel);
    }

    public void SetCurrentHealth(int currentHealth)
    {
        _viewModel.CurrentHealth = currentHealth;
    }
    public void SetMaxHealth(int maxHealth)
    {
        _viewModel.MaxHealth = maxHealth;
    }
}
