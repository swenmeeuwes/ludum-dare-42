using UnityEngine;
using Zenject;

public class CameraDarkness : MonoBehaviour {
    [SerializeField] private Material _darknessMaterial;

    #region Injected

    private Player _player;

    #endregion

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_darknessMaterial != null)
        {
            _darknessMaterial.SetFloat("_Darkness", Mathf.Clamp01(_player.FogCorruption));
            Graphics.Blit(source, destination, _darknessMaterial);
        }
    }
}
