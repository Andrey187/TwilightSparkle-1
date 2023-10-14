using System.Threading;
using static WaveSpawner;

public interface IWaveSpawner
{

    public Wave[] Waves { get; }
    public void BossSpawn(Wave wave) { }
    public CancellationTokenSource CancellationTokenSource { get; }
}
