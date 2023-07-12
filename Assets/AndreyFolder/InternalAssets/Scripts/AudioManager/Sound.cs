using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundEnum SoundType;
    public AudioClip Clip;

    public enum SoundEnum
    {
        BackgroundMusicFirstScene,
        BackgroundMusic,
        FireBall,
        FrostBall,
        ArcaneBall,
        EnemyDie,
        MeteorHit
    }
}


