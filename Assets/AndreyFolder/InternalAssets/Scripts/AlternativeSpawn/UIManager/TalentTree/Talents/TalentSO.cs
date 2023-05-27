using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Talents/New Talent")]
public class TalentSO : ScriptableObject, IResetOnExitPlay
{
    public string Name;
    public string Description;
    public int Cost;
    public int CurrentTalentPoint;
    public TalentStatType StatType;
    [SerializeField] private int StartingTalentPoint = 0; // Add a new property to store the initial value

    public void UpdateTalent(PlayerStats playerStats, int buttonValue)
    {
        PropertyInfo prop = typeof(PlayerStats).GetProperty(StatType.ToString());
        int currentValue = (int)prop.GetValue(playerStats);
        prop.SetValue(playerStats, currentValue + buttonValue);
    }

    public void ResetOnExitPlay()
    {
        CurrentTalentPoint = StartingTalentPoint;
    }
}
