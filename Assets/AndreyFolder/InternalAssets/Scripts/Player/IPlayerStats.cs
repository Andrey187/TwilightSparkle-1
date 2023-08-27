using System;

public interface IPlayerStats
{
    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }

    public int TalentPoints { get; set; }

    public int CurrentLevel { get; set; }

    public int CurrentExp { get; set; }

    public int Speed { get; set; }

    public int MagicPower { get; set; }

    public event Action SpeedChanged;
    public event Action<int> MagicPowerChanged;
}
