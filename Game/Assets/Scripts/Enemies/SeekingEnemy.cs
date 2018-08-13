using UnityEngine;
using Zenject;

/// <summary>
/// An enemy type which seeks the player
/// </summary>
[RequireComponent(typeof(ZenjectBinding))]
public class SeekingEnemy : Enemy
{
    #region Injected

    private Player _player; // the object the enemy seeks

    #endregion

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    protected override void Update()
    {
        base.Update();

        // Flip sprite accordingly (todo: in enemy base class?)
        var relativePositionToPlayer = _player.transform.position - transform.position;
        if (relativePositionToPlayer.x < -0.5)
        {
            EnemyBody.SpriteRenderer.flipX = true;
        } else if (relativePositionToPlayer.x > 0.5)
        {
            EnemyBody.SpriteRenderer.flipX = false;
        }

        if (IsBeingHit || IsDieing || IsAttacking)
            return;

        var directionToPlayer = (_player.transform.position - transform.position).normalized;
        transform.Translate(directionToPlayer * Time.deltaTime);
    }
}
