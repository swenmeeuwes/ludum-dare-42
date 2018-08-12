using Zenject;

class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<GameManager>()
            .AsSingle()
            .NonLazy();
    }
}