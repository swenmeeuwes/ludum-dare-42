using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemyBody : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHitAnimationFinished;
    [SerializeField] private UnityEvent _onDieAnimationFinished;

    public Animator Animator { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnHitAnimationFinished()
    {
        _onHitAnimationFinished.Invoke();
    }

    public void OnDieAnimationFinished()
    {
        _onDieAnimationFinished.Invoke();
    }
}
