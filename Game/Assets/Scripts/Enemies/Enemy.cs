using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private EnemyBody _enemyBody;

    #region Injected

    protected SignalBus SignalBus;
    private EnemySpawner _enemySpawner;

    #endregion

    protected bool IsBeingHit;
    protected bool IsDieing;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tag.Bullet.ToString())
        {
            // Bullet hit
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null && bullet.Owner == BulletOwner.Player && !bullet.DespawnPending)
            {
                // Entity was hit by a player bullet
                Damage(bullet.Damage);

                bullet.Hit(transform);
            }
        }
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
            _enemyBody.Animator.SetTrigger("Hit");            
        }
    }

    public virtual void Kill()
    {
        SignalBus.Fire(new EnemyKilledSignal
        {
            Enemy = this
        });

        IsDieing = true;
        _enemyBody.Animator.SetTrigger("Die");

        _enemyBody.SpriteRenderer.DOFade(0f, 0.5f);
    }

    public void OnHitAnimationFinished()
    {
        IsBeingHit = false;
    }

    public void OnDieAnimationFinished()
    {
        Destroy(gameObject);
    }
}
