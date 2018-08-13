using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySettings
{
    public int Health;
    public float Speed;
    public int PointsWorth;
    public float SecondsWorth;
    [Tooltip("The amount of seconds it takes from the fog shrink duration per second")] public float FogCorruptionPerSecond;
    [Range(0, 1)] public float CorruptionPerSecond;
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySettings _stats;
    [SerializeField] protected EnemyBody EnemyBody;

    #region Injected

    protected SignalBus SignalBus;
    private EnemySpawner _enemySpawner;
    private FogManager _fogManager;
    private PowerupFactory _powerupFactory;
    private GameplaySettings _gameplaySettings;

    #endregion

    public EnemySettings Stats {
        get { return _stats; }
        set { _stats = value; } 
    }

    public float SecondsWorth { get { return Stats.SecondsWorth; } }

    protected bool IsBeingHit;
    protected bool IsDieing;
    
    public bool IsAttacking { get { return _playerBeingAttacked != null; } }
    private Player _playerBeingAttacked;

    [Inject]
    private void Construct(SignalBus signalBus, EnemySpawner enemySpawner, FogManager fogManager, PowerupFactory powerupFactory, GameplaySettings gameplaySettings)
    {
        SignalBus = signalBus;
        _enemySpawner = enemySpawner;
        _fogManager = fogManager;
        _powerupFactory = powerupFactory;
        _gameplaySettings = gameplaySettings;
    }

    protected virtual void Update()
    {
        if (IsAttacking)
        {
            _fogManager.TakeTime(Stats.FogCorruptionPerSecond * Time.deltaTime);
            _playerBeingAttacked.Corrupt(Stats.CorruptionPerSecond * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _enemySpawner.Deregister(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tag.Bullet.ToString())
        {
            // Bullet hit
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null && bullet.Owner == BulletOwner.Player && !bullet.DespawnPending && !IsDieing)
            {
                // Entity was hit by a player bullet
                Damage(bullet.Damage);

                bullet.Hit(transform);
            }
        }
    }

    public virtual void Attack(Player player)
    {
        _playerBeingAttacked = player;
    }

    public virtual void StopAttack(Player player)
    {
        _playerBeingAttacked = null;
    }

    public virtual void Damage(int amount)
    {
        Stats.Health -= amount;

        if (Stats.Health <= 0)
        {
            Kill();
        }
        else
        {
            IsBeingHit = true;
            EnemyBody.Animator.SetTrigger("Hit");   
            Invoke("OnHitAnimationFinished", 0.1f); // hack
        }
    }

    public virtual void Kill()
    {
        SignalBus.Fire(new EnemyKilledSignal
        {
            Enemy = this
        });

        IsDieing = true;
        EnemyBody.Animator.SetTrigger("Die");

        EnemyBody.SpriteRenderer.DOFade(0f, 0.5f).OnComplete(OnDieAnimationFinished);
    }

    public void OnHitAnimationFinished()
    {
        IsBeingHit = false;
    }

    public void OnDieAnimationFinished()
    {
        if (Random.value <= _gameplaySettings.PowerupDropChance)
        {
            var randomPowerupType = (PowerupType) Random.Range(0, 1 + 1);
            var powerup = _powerupFactory.Create(randomPowerupType);
            powerup.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
