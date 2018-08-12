using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image _cutscenePanel;
    [SerializeField] private TMP_Text _cutsceneText;
    [SerializeField] private TMP_Text _tutorialText;

    [SerializeField] private SpriteRenderer[] _playerParts;
    [SerializeField] private float[] _playerPartsAlpha;

    #region Injected

    private SceneLoader _sceneLoader;

    #endregion

    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void Start()
    {
        _cutsceneText.alpha = 0;
        _tutorialText.alpha = 0;

        _playerPartsAlpha = new float[_playerParts.Length];
        var i = 0;
        foreach (var playerPart in _playerParts)
        {
            _playerPartsAlpha[i] = playerPart.color.a; // save to respect set alpha values in the inspector
            playerPart.color = new Color(playerPart.color.r, 
                playerPart.color.g, playerPart.color.b, 0);

            i++;
        }

        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        // HARD CODE BOYS
        _cutsceneText.text = "You are the light\n in the darkness";
        _cutsceneText.alpha = 0;
        _cutsceneText.DOFade(1f, 1.2f);

        yield return new WaitForSeconds(3f + 1.2f);

        _cutsceneText.DOFade(0f, 0.65f);

        yield return new WaitForSeconds(0.5f + 0.65f);

        _cutsceneText.text = "Shine bright";
        _cutsceneText.DOFade(1f, 1.2f);

        yield return new WaitForSeconds(2.5f + 1.2f);

        _cutsceneText.DOFade(0f, 0.65f);

        yield return new WaitForSeconds(0.25f + 0.65f);

        _cutscenePanel.DOFade(0f, 2f);

        yield return new WaitForSeconds(1f + 2f);

        for (var i = 0; i < _playerParts.Length; i++)
        {
            _playerParts[i].DOFade(_playerPartsAlpha[i], 0.65f);
        }

        yield return new WaitForSeconds(1f + 0.65f);

        _tutorialText.text = "This is you, the light";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(5f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(0.5f + 0.25f);

        _tutorialText.text = "Move around with the arrow keys, WASD or the left joystick";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(8f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(1.5f + 0.25f);

        _tutorialText.text = "Aim with the mouse or the right joystick";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(8f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(0.5f + 0.25f);

        _tutorialText.text = "Shoot with the left mouse button or right trigger";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(8f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(0.5f + 0.25f);

        _tutorialText.text = "Protect yourself against the darkness";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(5f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(0.5f + 0.25f);

        _tutorialText.text = "Spread your light";
        _tutorialText.DOFade(1f, 0.45f);

        yield return new WaitForSeconds(3f + 0.45f);

        _tutorialText.DOFade(0f, 0.25f);

        yield return new WaitForSeconds(3f + 0.25f);

        _cutscenePanel.DOFade(1f, 4f);

        yield return new WaitForSeconds(1f + 1.5f);

        // End
        _sceneLoader.LoadAsync(Scenes.Game);
    }
}
