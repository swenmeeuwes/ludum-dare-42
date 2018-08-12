using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class EnemySettings
{
    public int Health;
    public float Speed;
    public int PointsWorth;
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemySettings Stats;

    #region Injected

    protected SignalBus SignalBus;
    private EnemySpawner _enemySpawner;

    #endregion

    [Inject]
    private void Construct(SignalBus signalBus, EnemySpawner enemySpawner)
    {
        SignalBus = signalBus;
        _enemySpawner = enemySpawner;
    }

    private void OnDestroy()
    {
        _enemySpawner.Deregister(this);
    }

    public virtual void Damage(int amount)
    {
        Stats.Health -= amount;

        if (Stats.Health <= 0)
            Kill();
    }

    public virtual void Kill()
    {
        SignalBus.Fire(new EnemyKilledSignal
        {
            Enemy = this
        });

        Destroy(gameObject);
    }
}
