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

        if (IsBeingHit || IsDieing || IsAttacking)
            return;

        var directionToPlayer = (_player.transform.position - transform.position).normalized;
        transform.Translate(directionToPlayer * Time.deltaTime);
    }
}
