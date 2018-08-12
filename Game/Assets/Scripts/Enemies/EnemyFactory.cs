
using Zenject;

public class EnemyFactory
{
    #region Injected

    private readonly DiContainer _container;
    private readonly ProjectSettings _projectSettings;

    #endregion

    public EnemyFactory(DiContainer container, ProjectSettings projectSettings)
    {
        _container = container;
        _projectSettings = projectSettings;
    }

    public Enemy Create(EnemyType type)
    {
        switch (type)
        {
            default:
            case EnemyType.Slow:
                return _container.InstantiatePrefabForComponent<SeekingEnemy>(_projectSettings.Prefabs.SlowEnemy);
        }
    }
}
