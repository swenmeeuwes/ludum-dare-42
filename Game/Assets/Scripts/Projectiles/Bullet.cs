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

[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSprites _sprites;

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
    public Vector2 Velocity { get; set; }

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Velocity * Time.deltaTime);
    }

    public class Pool : MemoryPool<BulletOwner, Vector2, Vector2, Bullet>
    {
        protected override void Reinitialize(BulletOwner owner, Vector2 position, Vector2 velocity, Bullet item)
        {
            base.Reinitialize(owner, position, velocity, item);

            item.Owner = owner;
            item.transform.position = position;
            item.Velocity = velocity;
        }
    }
}
