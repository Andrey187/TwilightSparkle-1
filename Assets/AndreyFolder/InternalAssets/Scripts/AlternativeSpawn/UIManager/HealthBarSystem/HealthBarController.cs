using UnityEngine;

public class HealthBarController : MonoCache
{
    [SerializeField] public HealthBarView _healthBarView;
    private HealthBarViewModel _viewModel;

    public void Initialize(int maxHealth)
    {
        var model = Get<HealthBarModel>();
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
