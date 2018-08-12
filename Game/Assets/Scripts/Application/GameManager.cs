using System.Collections;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    #region Injected

    private Player _player;
    private FogManager _fogManager;
    private MonoBehaviourUtil _monoBehaviourUtil;
    private GameplaySettings _gameplaySettings;
    private EnemySpawner _enemySpawner;

    #endregion

    private float _timeStarted;

    [Inject]
    private void Construct(Player player, FogManager fogManager, MonoBehaviourUtil monoBehaviourUtil, GameplaySettings gameplaySettings, EnemySpawner enemySpawner)
    {
        _player = player;
        _fogManager = fogManager;
        _monoBehaviourUtil = monoBehaviourUtil;
        _gameplaySettings = gameplaySettings;
        _enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        _monoBehaviourUtil.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        _player.Enabled = false;
        _player.FadeIn(5f, 0.65f);

        yield return new WaitForSeconds(2f + 5f);

        _player.Enabled = true;

        yield return new WaitForSeconds(1f);

        _fogManager.Shrinking = true;

        _timeStarted = Time.time;

        yield return new WaitForSeconds(1f);

        _monoBehaviourUtil.StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        var nextSpawn = _gameplaySettings.SpawnCurve.Evaluate(Time.time - _timeStarted);

        _enemySpawner.Spawn();

        yield return new WaitForSeconds(nextSpawn);

        _monoBehaviourUtil.StartCoroutine(SpawnLoop());
    }
}
