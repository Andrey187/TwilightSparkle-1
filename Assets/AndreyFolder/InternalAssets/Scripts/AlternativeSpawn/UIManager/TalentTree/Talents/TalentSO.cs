using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Talents/New Talent")]
public class TalentSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int Cost;
    public int CurrentTalentPoint;
    public TalentStatType StatType;

    private void OnDisable()
    {
        CurrentTalentPoint = 0;
    }

    public void UpdateTalent(PlayerStats playerStats, int buttonValue)
    {
        PropertyInfo prop = typeof(PlayerStats).GetProperty(StatType.ToString());
        int currentValue = (int)prop.GetValue(playerStats);
        prop.SetValue(playerStats, currentValue + buttonValue);
    }
}
