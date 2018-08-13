using Zenject;

public class PowerupFactory
{
    #region Injected

    private DiContainer _container;
    private ProjectSettings _projectSettings;

    #endregion

    public PowerupFactory(DiContainer container, ProjectSettings settings)
    {
        _container = container;
        _projectSettings = settings;
    }

    public Powerup Create(PowerupType type)
    {
        var powerup = _container.InstantiatePrefabForComponent<Powerup>(_projectSettings.Prefabs.Powerup);
        powerup.Type = type;

        return powerup;
    }
}