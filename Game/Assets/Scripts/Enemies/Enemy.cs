﻿using System;
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

    #endregion

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        SignalBus = signalBus;
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
