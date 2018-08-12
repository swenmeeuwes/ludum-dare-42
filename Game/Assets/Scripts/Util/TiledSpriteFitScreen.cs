using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteRenderer))]
public class TiledSpriteFitScreen : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer.drawMode != SpriteDrawMode.Sliced)
        {
            Debug.LogWarningFormat("[TiledSpriteFitScreen] {0} is not a nine-sliced tile!", gameObject);
        }
        else
        {
            var pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
            _spriteRenderer.size = new Vector2(pixelPerfectCamera.refResolutionX / (float)pixelPerfectCamera.assetsPPU, 
                pixelPerfectCamera.refResolutionY / (float)pixelPerfectCamera.assetsPPU);
        }
    }
}
