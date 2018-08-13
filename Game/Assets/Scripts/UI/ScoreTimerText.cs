using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenjectBinding), typeof(TMP_Text))]
public class ScoreTimerText : MonoBehaviour
{
    private TMP_Text _textField;

    #region Injected

    private GameManager _gameManager;

    #endregion

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _textField = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_gameManager.State == GameState.Playing)
            _textField.text = TimeUtil.SecondsToDigitalClock(Time.time - _gameManager.TimeStarted);
    }
}
