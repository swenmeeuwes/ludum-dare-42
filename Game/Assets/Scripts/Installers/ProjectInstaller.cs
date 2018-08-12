using DG.Tweening;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private ProjectSettings _settings;

    public override void InstallBindings()
    {
        DOTween.Init();
        SignalBusInstaller.Install(Container);

        #region Bindings

        Container
            .Bind<MonoBehaviourUtil>()
            .FromNewComponentOnNewGameObject()
            .AsSingle();

        Container
            .Bind<ProjectSettings>()
            .FromInstance(_settings)
            .AsSingle();

        Container
            .Bind<ControllerSettings>()
            .FromInstance(_settings.ControllerSettings)
            .AsSingle();

        Container
            .Bind<GameplaySettings>()
            .FromInstance(_settings.GameplaySettings)
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<BulletManager>()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<InputManager>()
            .AsSingle();            

        Container
            .BindInterfacesAndSelfTo<ScreenFactory>()
            .AsSingle()
            .WithArguments(_settings.Prefabs.ScreenRoot);

        Container
            .Bind<ScreenContext>()
            .FromInstance(_settings.ScreenContext)
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<NavigationManager>()
            .AsSingle();

        Container
            .Bind<SceneLoader>()
            .ToSelf()
            .AsSingle();

        #endregion

        #region Signals

        Container.DeclareSignal<OpenScreenRequestSignal>();
        Container.DeclareSignal<CloseScreenRequestSignal>();
        Container.DeclareSignal<CloseAllScreensRequestSignal>();
        Container.DeclareSignal<ScreenStateChangedSignal>();

        Container.DeclareSignal<InputTypeChangedSignal>().OptionalSubscriber();

        #endregion

        #region Commands

        Container.DeclareSignal<LoadSceneSignal>();
        Container.Bind<LoadSceneCommand>().AsTransient();
        Container
            .BindSignal<LoadSceneSignal>()
            .ToMethod<LoadSceneCommand>(command => command.Execute)
            .FromResolve();

        #endregion

        #region MemoryPools

        Container
            .BindMemoryPool<Bullet, Bullet.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(_settings.Prefabs.Bullet)
            .UnderTransformGroup("Bullets");        

        #endregion
    }
}
