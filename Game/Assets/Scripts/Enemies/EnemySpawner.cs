using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : IInitializable
{
    #region Injected

    private GameplaySettings _gameplaySettings;
    private EnemyFactory _enemyFactory;

    #endregion

    private List<Enemy> _enemies = new List<Enemy>();
    private Transform _enemyRoot;
    private Rect _spawnBounds;

    public EnemySpawner(GameplaySettings gameplaySettings, EnemyFactory enemyFactory)
    {
        _gameplaySettings = gameplaySettings;
        _enemyFactory = enemyFactory;
    }

    public void Initialize()
    {
        _enemyRoot = new GameObject("Enemies").transform;
        _spawnBounds = PixelPerfectCameraUtil.Bounds.Expand(1f);
    }

    public void Spawn()
    {
        if (_enemies.Count + 1 > _gameplaySettings.MaxEnemies)
            return;

        var enemy = GenerateRandomEnemy();
        enemy.transform.SetParent(_enemyRoot);
        _enemies.Add(enemy);

        // randomized spawn coördinates
        float x = 0;
        float y = 0;

        var side = (Side)Random.Range(0, 3 + 1);
        switch (side)
        {
            case Side.Top:
                x = Random.Range(_spawnBounds.x, _spawnBounds.width);
                y = _spawnBounds.height;
                break;
            case Side.Bottom:
                x = Random.Range(_spawnBounds.x, _spawnBounds.width);
                y = _spawnBounds.y;
                break;
            case Side.Left:
                x = _spawnBounds.x;
                y = Random.Range(_spawnBounds.y, _spawnBounds.height);
                break;
            case Side.Right:
                x = _spawnBounds.width;
                y = Random.Range(_spawnBounds.y, _spawnBounds.height);
                break;
        }

        enemy.transform.position = new Vector2(x, y);
    }

    public void Deregister(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    private Enemy GenerateRandomEnemy()
    {
        var enemyType = (EnemyType)Random.Range(0, 2 + 1);
        switch (enemyType)
        {
            case EnemyType.Fast:
                return _enemyFactory.Create(EnemyType.Fast);
            case EnemyType.Shooting:
                return _enemyFactory.Create(EnemyType.Shooting);
            case EnemyType.Slow:
            default:
                return _enemyFactory.Create(EnemyType.Slow);
        }
    }
}