using AbilitySystem;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerStats>().To<PlayerStats>().FromComponentInHierarchy().AsSingle().Lazy();

        Container.Bind<ILevelUpSystem>().To<LevelUpSystem>().FromComponentInHierarchy().AsSingle().Lazy();

        Container.Bind<IMovementSpeedModifier>().To<SlowMovementSpeedModifier>().AsSingle().Lazy();

        Container.Bind<IAbilitySpawn>().To<AbilitiesSpawn>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<PositionWritter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MagicShield>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IAttackSystem>().To<AttackSystem>().FromComponentInHierarchy().AsSingle().Lazy();

        Container.Bind<MultipleFireBallAbility>().FromComponentInHierarchy().AsSingle();

    }
}
