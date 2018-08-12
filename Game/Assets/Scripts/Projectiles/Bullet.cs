using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum BulletOwner
{
    Enemy,
    Player
}

[Serializable]
public class BulletSprites
{
    public Sprite PlayerBulletSprite;
    public Sprite EnemyBulletSprite;
}

[Serializable]
public class BulletParticleMaterials
{
    public Material PlayerParticleMaterial;
    public Material EnemyParticleMaterial;
}

[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _torque;
    [SerializeField] private GameObject _bulletSplashPrefab;
    [SerializeField] private BulletSprites _sprites;
    [SerializeField] private BulletParticleMaterials _particleMaterials;

    private BulletOwner _owner;
    public BulletOwner Owner {
        get { return _owner; }
        set
        {
            _owner = value;

            switch (value)
            {
                case BulletOwner.Enemy:
                    _spriteRenderer.sprite = _sprites.EnemyBulletSprite;                    
                    break;
                case BulletOwner.Player:
                    _spriteRenderer.sprite = _sprites.PlayerBulletSprite;                    
                    break;
            }            
        }
    }
    public bool DespawnPending { get; set; }
    public Vector2 Velocity { get; set; }
    public int Damage = 1;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Velocity * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(0, 0, _torque * Time.deltaTime), Space.World);
    }

    public void Hit(Transform other)
    {
        SpawnBulletSplash();
        DespawnPending = true;
    }

    public class Pool : MonoMemoryPool<BulletOwner, Vector2, Vector2, Bullet>
    {
        protected override void Reinitialize(BulletOwner owner, Vector2 position, Vector2 velocity, Bullet item)
        {
            base.Reinitialize(owner, position, velocity, item);

            item.Owner = owner;
            item.transform.position = position;
            item.Velocity = velocity;
        }
    }

    private void SpawnBulletSplash()
    {
        var bulletSplash = Instantiate(_bulletSplashPrefab);
        bulletSplash.transform.position = transform.position;

        var particleSystem = bulletSplash.GetComponent<ParticleSystem>();
        var splashRenderer = bulletSplash.GetComponent<Renderer>();

        switch (Owner)
        {
            case BulletOwner.Enemy:
                splashRenderer.material = _particleMaterials.EnemyParticleMaterial;
                break;
            case BulletOwner.Player:
                splashRenderer.material = _particleMaterials.PlayerParticleMaterial;
                break;
        }

        particleSystem.Play();
    }
}
