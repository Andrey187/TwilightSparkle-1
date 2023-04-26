using UnityEngine;
using System.ComponentModel;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] public HealthBar _healthBar;
    private HealthBarViewModel _viewModel;

    public void SetViewModel(HealthBarViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += HandlePropertyChanged;
        _healthBar.SetMaxHealth(_viewModel.MaxHealth);
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HealthBarViewModel.CurrentHealth))
        {
            _healthBar.SetHealth(_viewModel.CurrentHealth);
        }
    }
}
