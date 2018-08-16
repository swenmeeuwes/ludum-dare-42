using System.Collections;
using UnityEngine;
using Zenject;

public class StartScreenController : MonoBehaviour
{
    [Tooltip("Scene to load after start screen")][SerializeField] private Scenes _nextScene;

    #region Injected

    private SignalBus _signalBus;

    #endregion

    private bool _isLoading = false; // to prevent the loading of more than one scene

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Update()
    {
        if ((Input.anyKey ||
            Input.GetButton(InputAxes.Submit)
            ) 
            && !_isLoading)
        {
            _isLoading = true;
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);

        _signalBus.Fire(new LoadSceneSignal
        {
            Scene = _nextScene
        });
    }
}
