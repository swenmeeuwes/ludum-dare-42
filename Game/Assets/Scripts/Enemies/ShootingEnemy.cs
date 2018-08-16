using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// An enemy type which seeks the player and fires bullets at it when in range
/// </summary>
[RequireComponent(typeof(ZenjectBinding), typeof(AudioSource))]
public class ShootingEnemy : Enemy
{
    [SerializeField] private Transform _bulletOrigin;
    [SerializeField] private float _maxDistanceFromPlayer = 1f;
    [SerializeField] private float _fireRate = 1f; // N bullet per second
    [SerializeField] private float _bulletSpeed = 10f;

    #region Injected

    private Player _player; // the object the enemy seeks
    private BulletManager _bulletManager;

    #endregion

    private AudioSource _audioSource;

    private float _lastShot;

    [Inject]
    private void Construct(Player player, BulletManager bulletManager)
    {
        _player = player;
        _bulletManager = bulletManager;
    }

    protected void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _lastShot = Time.time;
    }

    protected override void Update()
    {
        base.Update();

        // Flip sprite accordingly (todo: in enemy base class?)
        var relativePositionToPlayer = _player.transform.position - transform.position;
        if (relativePositionToPlayer.x < -0.5)
        {
            EnemyBody.SpriteRenderer.flipX = true;
        }
        else if (relativePositionToPlayer.x > 0.5)
        {
            EnemyBody.SpriteRenderer.flipX = false;
        }

        if (IsBeingHit || IsDieing || IsAttacking)
            return;

        var vectorToPlayer = _player.transform.position - transform.position;
        if (vectorToPlayer.magnitude > _maxDistanceFromPlayer)
        {            
            transform.Translate(vectorToPlayer.normalized * Stats.Speed * Time.deltaTime);
        }
        else if (vectorToPlayer.magnitude < _maxDistanceFromPlayer - 0.2f)
        {
            // Walk away when the player comes too close
            transform.Translate(-vectorToPlayer.normalized * Stats.Speed * Time.deltaTime);
        }

        Shoot();
    }

    private void Shoot()
    {
        if (Time.time - _lastShot < _fireRate)
            return;

        _lastShot = Time.time;

        StartCoroutine(ShootSequence());
    }

    private IEnumerator ShootSequence()
    {
        var bulletOrigin = _bulletOrigin.position;
        if (EnemyBody.SpriteRenderer.flipX) // take in account the sprite flip
            bulletOrigin.x -= _bulletOrigin.localPosition.x;

        var vectorToPlayer = _player.transform.position - bulletOrigin;

        for (var i = 0; i < Random.Range(2, 4); i++)
        { 
            _bulletManager.Create(BulletOwner.Enemy,
                bulletOrigin, vectorToPlayer.normalized * _bulletSpeed);
            _audioSource.Play();

            yield return new WaitForSeconds(0.1f);
        }
    }
}