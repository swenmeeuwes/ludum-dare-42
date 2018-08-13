using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverScreenController : ScreenController
{
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private CanvasGroup[] _buttonImageGroups;

    #region Injected

    private SignalBus _signalBus;
    private JoystickEventSystem _joystickEventSystem;
    private SceneLoader _sceneLoader;

    #endregion

    [Inject]
    private void Construct(SignalBus signalBus, JoystickEventSystem joystickEventSystem, SceneLoader sceneLoader)
    {
        _signalBus = signalBus;
        _joystickEventSystem = joystickEventSystem;
        _sceneLoader = sceneLoader;
    }

    private void Start()
    {
        _gameOverText.color = new Color(_gameOverText.color.r,
            _gameOverText.color.g, _gameOverText.color.b, 0);

        foreach (var buttonImage in _buttonImageGroups)
        {
            buttonImage.alpha = 0;
        }

        foreach (var button in _buttons)
        {            
            button.gameObject.SetActive(false);
        }

        StartCoroutine(GameOverSequence());
    }

    // Called from the inspector
    public void HandleRetryButton()
    {
        _signalBus.Fire(new CloseAllScreensRequestSignal
        {
            ForceInstant = true
        });

        _sceneLoader.LoadAsync(Scenes.Game);
    }

    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(0.5f);

        _gameOverText.text = "You've been consumed by the darkness";
        _gameOverText.DOFade(1f, 1.5f);

        yield return new WaitForSeconds(1f + 1.5f);

        _gameOverText.DOFade(0f, 0.45f);

        yield return new WaitForSeconds(0.5f + 0.45f);

        _gameOverText.text = "Your light shines no more";
        _gameOverText.DOFade(1f, 1.5f);

        yield return new WaitForSeconds(0.5f + 1.5f);

        _gameOverText.DOFade(0f, 0.45f);

        yield return new WaitForSeconds(0.5f + 0.45f);

        _gameOverText.text = string.Format("Game over\nYou survived for {0}", TimeUtil.SecondsToDigitalClock(GameManager.TimeScore));
        _gameOverText.DOFade(1f, 1.5f);

        yield return new WaitForSeconds(1f + 1.5f);

        foreach (var button in _buttons)
        {

            button.gameObject.SetActive(true);
        }

        foreach (var buttonImage in _buttonImageGroups)
        {
            buttonImage.DOFade(1f, 0.45f);
        }

        yield return new WaitForSeconds(0.45f);

        _joystickEventSystem.ScanForButtons();

        if (_buttons.Length > 0)
            _joystickEventSystem.SetSelectedGameObject(_buttons[0].gameObject);
    }
}
