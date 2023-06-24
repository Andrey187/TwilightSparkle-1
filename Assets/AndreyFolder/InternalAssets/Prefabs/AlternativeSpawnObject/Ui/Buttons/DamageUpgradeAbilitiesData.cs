using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DamageUpgradeAbilitiesData : MonoBehaviour
{
    public Dictionary<UpgradeAbilitiesOnButtonClick, int> _storeDamage = new Dictionary<UpgradeAbilitiesOnButtonClick, int>();
    private AbilityAddWindow _abilityAddWindow;
    private PlayerStats _playerStats;
    private void Start()
    {
        _abilityAddWindow = FindObjectOfType<AbilityAddWindow>();
        _playerStats = FindObjectOfType<PlayerStats>();
        foreach (var button in _abilityAddWindow.ShuffleButtons)
        {
            if (button.TryGetComponent<UpgradeAbilitiesOnButtonClick>(out var upgradeButton))
            {
                int _damageIncrease = upgradeButton.DamageIncrease;
                _storeDamage.Add(upgradeButton, _damageIncrease);
                upgradeButton.PropertyChanged += UpgradeButton_PropertyChanged;
            }
        }
        _playerStats.MagicPowerChanged += DamageUp;
    }

   
    public void DamageUp(int damage)
    {
        // Create a separate list of keys to update
        List<UpgradeAbilitiesOnButtonClick> keysToUpdate = new List<UpgradeAbilitiesOnButtonClick>();

        // Collect the keys to update
        foreach (var kvp in _storeDamage)
        {
            var upgradeButton = kvp.Key;
            keysToUpdate.Add(upgradeButton);
        }

        // Update the values for the collected keys
        foreach (var upgradeButton in keysToUpdate)
        {
            var currentDamageIncrease = _storeDamage[upgradeButton];

            // Calculate the new damage increase using the formula
            int newDamageIncrease = currentDamageIncrease + Mathf.RoundToInt(currentDamageIncrease * damage / 100f);

            // Update the value in the dictionary for the current key
            upgradeButton.DamageIncrease = newDamageIncrease;
        }
    }

    private void UpgradeButton_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UpgradeAbilitiesOnButtonClick.DamageIncrease))
        {
            var upgradeButton = (UpgradeAbilitiesOnButtonClick)sender;
            _storeDamage[upgradeButton] = upgradeButton.DamageIncrease;
        }
    }

    private void OnDestroy()
    {
        foreach (var button in _storeDamage.Keys)
        {
            button.PropertyChanged -= UpgradeButton_PropertyChanged;
        }

        _storeDamage.Clear();
        _playerStats.MagicPowerChanged -= DamageUp;
    }
}
