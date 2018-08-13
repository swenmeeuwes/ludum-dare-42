using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class HintText : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private float _floatDistance; // over y axis
    [SerializeField] private float _floatDuration;

    public string Text {
        set { _textField.text = value; }
    }

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        var endY = transform.position.y + _floatDistance;
        transform.DOMoveY(endY, _floatDuration);

        yield return new WaitForSeconds(0.8f);

        _textField.DOFade(0f, _floatDuration).OnComplete(() => Destroy(gameObject));
    }
}
