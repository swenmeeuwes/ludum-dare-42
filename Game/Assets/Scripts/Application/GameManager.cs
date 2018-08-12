using System.Collections;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    #region Injected

    private Player _player;
    private FogManager _fogManager;
    private MonoBehaviourUtil _monoBehaviourUtil;

    #endregion

    [Inject]
    private void Construct(Player player, FogManager fogManager, MonoBehaviourUtil monoBehaviourUtil)
    {
        _player = player;
        _fogManager = fogManager;
        _monoBehaviourUtil = monoBehaviourUtil;
    }

    public void Initialize()
    {
        _monoBehaviourUtil.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        _player.Enabled = false;
        _player.FadeIn(5f, 0.65f);

        yield return new WaitForSeconds(2f + 5f);

        _player.Enabled = true;
        _fogManager.Shrinking = true;
    }
}
