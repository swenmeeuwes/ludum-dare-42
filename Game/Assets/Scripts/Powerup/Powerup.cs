using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
public class Powerup : MonoBehaviour
{
    #region Injected

    private HintTextFactory _hintTextFactory;

    #endregion


    public PowerupType Type { get; set; }
    public bool Used { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    [Inject]
    private void Construct(HintTextFactory hintTextFactory)
    {
        _hintTextFactory = hintTextFactory;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Use()
    {
        if (!Used)
            _audioSource.Play();

        Used = true;

        var hintText = _hintTextFactory.Create("+" + Type);
        hintText.transform.position = transform.position + Vector3.up * 0.5f;        

        _spriteRenderer.DOFade(0f, 0.45f).OnComplete(() => Destroy(gameObject));
    }
}
