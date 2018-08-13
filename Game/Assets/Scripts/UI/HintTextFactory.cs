using Zenject;

public class HintTextFactory
{
    #region Injected

    private DiContainer _container;
    private ProjectSettings _projectSettings;

    #endregion

    public HintTextFactory(DiContainer container, ProjectSettings projectSettings)
    {
        _container = container;
        _projectSettings = projectSettings;
    }

    public HintText Create(string text)
    {
        var hintText = _container.InstantiatePrefabForComponent<HintText>(_projectSettings.Prefabs.HintText);
        hintText.Text = text;

        return hintText;
    }
}