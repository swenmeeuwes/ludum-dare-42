using Zenject;

class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        #region Bindings

        Container
            .BindInterfacesAndSelfTo<GameManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<EnemyFactory>()
            .ToSelf()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<EnemySpawner>()
            .AsSingle();

        #endregion

        #region Signals

        Container
            .DeclareSignal<EnemyKilledSignal>()
            .OptionalSubscriber();

        #endregion
    }
}