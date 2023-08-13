using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class TalentButtonManager : MonoBehaviour
{
    [SerializeField] private TalentSystem _talentSystem;
    [SerializeField] private LevelUpSystem levelUpSystem;
    [SerializeField] private Button speedBoostButton;
    [SerializeField] private Button damageBoostButton;

    private bool speedButtonActive = false;
    private bool damageButtonActive = false;

    private void Start()
    {
        // Subscribe to talent system events
        speedBoostButton.interactable = false;
        damageBoostButton.interactable = false;
        _talentSystem.PropertyChanged += HandlePropertyChanged;
        levelUpSystem.PropertyChanged += HandlePropertyChanged; 
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        levelUpSystem.PropertyChanged -= HandlePropertyChanged;
        _talentSystem.PropertyChanged -= HandlePropertyChanged;
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(levelUpSystem.TalentPoint):
                if(levelUpSystem.TalentPoint != 0 && _talentSystem.TalentPoints != 0)
                {
                    UpdateButtonActivity();
                }
                break;
            case nameof(_talentSystem.TalentPoints):

                if (levelUpSystem.TalentPoint != 0 && _talentSystem.TalentPoints != 0)
                {
                    UpdateButtonActivity();
                }
                break;
        }
    }

    private void UpdateButtonActivity()
    {
        // Check the current talent points
        Dictionary<TalentStatType, int> talentPoints = _talentSystem.CurrentTalentPointsValue;

        // Check if speed button should be activated
        if (!speedButtonActive)
        {
            bool speedButtonActivated = talentPoints.TryGetValue(TalentStatType.MaxHealth, out int hpPoints) && hpPoints != 0 && hpPoints % 2 == 0 && !damageButtonActive;
            speedBoostButton.interactable = speedButtonActivated;
        }

        if (!damageButtonActive)
        {
            bool damageButtonActivated =/* talentPoints.TryGetValue(TalentStatType.MaxHealth, out int hpPoints) && hpPoints % 2 == 0 &&*/
            talentPoints.TryGetValue(TalentStatType.Speed, out int speedPoints) && speedPoints >= 1 && !speedBoostButton.interactable;
            damageBoostButton.interactable = damageButtonActivated;
            damageBoostButton.onClick.AddListener(InvestDamagePoint);
        }
        speedButtonActive = false;
        damageButtonActive = false;
    }

    public void InvestSpeedPoint()
    {
        speedButtonActive = true;
        speedBoostButton.interactable = false;
    }

    public void InvestDamagePoint()
    {
        damageButtonActive = true;
        damageBoostButton.interactable = false;
    }

}
