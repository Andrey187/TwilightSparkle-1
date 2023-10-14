using AbilitySystem;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerStats>().To<PlayerStats>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<ILevelUpSystem>().To<LevelUpSystem>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<IAttackSystem>().To<AttackSystem>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<IAbilitySpawn>().To<AbilitiesSpawn>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<ICounters>().To<Counters>().FromComponentInHierarchy().AsSingle().Lazy();

        Container.Bind<PositionWritter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MagicShield>().FromComponentInHierarchy().AsSingle();


        Container.Bind<IMovementSpeedModifier>().To<SlowMovementSpeedModifier>().AsSingle().Lazy();

        Container.Bind<MultipleFireBallAbility>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IWaveSpawner>().To<WaveSpawner>().FromComponentInHierarchy().AsTransient();
        Container.Bind<IGamePause>().To<GamePause>().FromComponentInHierarchy().AsTransient();

    }
}
