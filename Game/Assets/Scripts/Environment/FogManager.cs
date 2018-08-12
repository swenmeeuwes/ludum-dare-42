using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

[Serializable]
public class FogObjects
{
    public Fog Top;
    public Fog Bottom;
    public Fog Left;
    public Fog Right;
}

public class FogManager : MonoBehaviour
{
    [SerializeField] private FogObjects _fogObjects;

    #region Injected

    private GameplaySettings _gameplaySettings;

    #endregion

    public Rect ScreenBounds { get; private set; } // todo: Compute this in a Camera class?

    private bool _shrinking;
    public bool Shrinking {
        get { return _shrinking; }
        set
        {
            _shrinking = value;
            _variableStartTime = Time.time;
        }
    }

    private Rect _shrinkStep; // per second
    private float _variableStartTime; // will be adjusted when extra time is granted to the player (so it is not the ACTUAL start time)

    [Inject]
    private void Construct(GameplaySettings gameplaySettings)
    {
        _gameplaySettings = gameplaySettings;
    }

    private void Start()
    {
        ScreenBounds = ComputeScreenBounds();
        _shrinkStep = ComputeShrinkStep();

        _fogObjects.Top.SetPosition(new Vector2(0, ScreenBounds.height));
        _fogObjects.Bottom.SetPosition(new Vector2(0, ScreenBounds.y));
        _fogObjects.Left.SetPosition(new Vector2(ScreenBounds.x, 0));
        _fogObjects.Right.SetPosition(new Vector2(ScreenBounds.width, 0));

        _variableStartTime = Time.time;
    }

    private void Update()
    {
        if (Shrinking)
            Shrink();
    }

    public void GrantTime(float seconds)
    {
        _variableStartTime += seconds;
    }

    public void TakeTime(float seconds)
    {
        _variableStartTime -= seconds;
    }

    private Rect ComputeScreenBounds()
    {
        // Standard camera
        //var cameraSize = Camera.main.orthographicSize;
        //var cameraHeight = cameraSize * 2f;
        //var cameraWidth = cameraSize * Camera.main.aspect;
        //return new Rect(-cameraWidth, -cameraHeight / 2f, cameraWidth, cameraHeight / 2f);        

        // Pixel perfect camera
        var pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
        var cameraWidth = pixelPerfectCamera.refResolutionX / pixelPerfectCamera.assetsPPU;
        var cameraHeight = pixelPerfectCamera.refResolutionY / pixelPerfectCamera.assetsPPU;

        return new Rect(-cameraWidth / 2f, -cameraHeight / 2f, cameraWidth / 2f, cameraHeight / 2f);
    }

    private Rect ComputeShrinkStep()
    {
        return new Rect(
            ScreenBounds.x / _gameplaySettings.FogShrinkDuration,
            ScreenBounds.y / _gameplaySettings.FogShrinkDuration,
            ScreenBounds.width / _gameplaySettings.FogShrinkDuration,
            ScreenBounds.height / _gameplaySettings.FogShrinkDuration
        );
    }

    private void Shrink()
    {
        var timeSinceStart = Time.time - _variableStartTime + 0.0001f; // + 0.0001f to prevent division by 0
        var interpolation = Mathf.Lerp(1, 0.0001f, timeSinceStart / _gameplaySettings.FogShrinkDuration);
        var newFogBounds = new Rect(
            ScreenBounds.x * interpolation,
            ScreenBounds.y * interpolation,
            ScreenBounds.width * interpolation,
            ScreenBounds.height * interpolation
        );

        _fogObjects.Top.SetPosition(new Vector2(0, newFogBounds.height));
        _fogObjects.Bottom.SetPosition(new Vector2(0, newFogBounds.y));
        _fogObjects.Left.SetPosition(new Vector2(newFogBounds.x, 0));
        _fogObjects.Right.SetPosition(new Vector2(newFogBounds.width, 0));
    }
}
