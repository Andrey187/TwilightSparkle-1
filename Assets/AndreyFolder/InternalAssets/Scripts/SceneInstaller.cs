using AbilitySystem;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Bindings installed!");
        Container.Bind<IPlayerStats>().To<PlayerStats>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<ILevelUpSystem>().To<LevelUpSystem>().FromComponentInHierarchy().AsSingle().Lazy();
        Container.Bind<IMovementSpeedModifier>().To<SlowMovementSpeedModifier>().AsSingle().Lazy();
        Container.Bind<PositionWritter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MagicShield>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IAttackSystem>().To<AttackSystem>().FromComponentInHierarchy().AsSingle().Lazy();

        Container.Bind<MultipleFireBallAbility>().FromComponentInHierarchy().AsSingle();

    }
}
