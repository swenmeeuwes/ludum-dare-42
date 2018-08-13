using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BulletManager : ITickable
{
    #region Injected

    private Bullet.Pool _bulletPool;

    #endregion

    private List<Bullet> _bullets = new List<Bullet>();

    public BulletManager(Bullet.Pool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public Bullet Create(BulletOwner owner, Vector2 position, Vector2 velocity, int damage = 1)
    {
        var bullet = _bulletPool.Spawn(owner, position, velocity, damage);
        _bullets.Add(bullet);

        return bullet;
    }

    public void Destroy(Bullet bullet)
    {
        bullet.DespawnPending = false;
        _bullets.Remove(bullet);
        _bulletPool.Despawn(bullet);
    }

    public void Tick()
    {
        _bullets.ForEach(bullet =>
        {
            if (!ScreenUtil.WorldPositionIsInView(bullet.transform.position) || bullet.DespawnPending)
                Destroy(bullet);
        });
    }
}
