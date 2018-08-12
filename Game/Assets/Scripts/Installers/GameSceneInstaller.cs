using Zenject;

class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
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
    }
}