using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    #region Injected

    private SignalBus _signalBus;
    private Player _player;
    private FogManager _fogManager;
    private MonoBehaviourUtil _monoBehaviourUtil;
    private GameplaySettings _gameplaySettings;
    private EnemySpawner _enemySpawner;

    #endregion

    public GameState State { get; set; }

    public float TimeStarted { get; private set; }
    public static float TimeScore { get; private set; } // will be set once game over + quick static hack..

    [Inject]
    private void Construct(SignalBus signalBus, Player player, FogManager fogManager, MonoBehaviourUtil monoBehaviourUtil, GameplaySettings gameplaySettings, EnemySpawner enemySpawner)
    {
        _signalBus = signalBus;
        _player = player;
        _fogManager = fogManager;
        _monoBehaviourUtil = monoBehaviourUtil;
        _gameplaySettings = gameplaySettings;
        _enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        State = GameState.Idle;

        _monoBehaviourUtil.StartCoroutine(StartGame());

        _signalBus.Subscribe<EnemyKilledSignal>(OnEnemyKilled);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyKilledSignal>(OnEnemyKilled);
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
            return;

        TimeScore = Time.time - TimeStarted;

        State = GameState.GameOver;

        _player.Enabled = false;

        _signalBus.Fire(new OpenScreenRequestSignal
        {
            Type = ScreenType.GameOver
        });
    }

    private void OnEnemyKilled(EnemyKilledSignal signal)
    {
        _fogManager.GrantTime(signal.Enemy.SecondsWorth);
    }

    private IEnumerator StartGame()
    {
        _player.Enabled = false;
        _player.FadeIn(3f, 0.65f);

        yield return new WaitForSeconds(1f + 3f);

        _player.Enabled = true;

        yield return new WaitForSeconds(1f);

        _fogManager.Shrinking = true;

        TimeStarted = Time.time;
        State = GameState.Playing;

        yield return new WaitForSeconds(1f);

        _monoBehaviourUtil.StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        if (State != GameState.Playing)
            yield break;

        var timePlaying = Time.time - TimeStarted;
        var nextSpawn = _gameplaySettings.SpawnCurve.Evaluate(timePlaying);
        var spawnAmount = Mathf.RoundToInt(_gameplaySettings.SpawnAmountCurve.Evaluate(timePlaying));

        for (var i = 0; i < spawnAmount; i++)
        {            
            _enemySpawner.Spawn();
        }

        yield return new WaitForSeconds(nextSpawn);

        _monoBehaviourUtil.StartCoroutine(SpawnLoop());
    }
}
