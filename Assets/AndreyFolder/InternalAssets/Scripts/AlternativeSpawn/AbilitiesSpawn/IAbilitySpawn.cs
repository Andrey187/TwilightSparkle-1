using System;

public interface IAbilitySpawn
{
    public event Action<Sound.SoundEnum> PlaySound;
}
