using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    [Tooltip("Rate of fire in seconds")]
    public float FireRate;

    public float BulletSpeed;

    [Tooltip("How long (seconds) can the player stand in the fog?")]
    public float FogSurvivalTime;
    [Tooltip("Restore rate per second")]
    [Range(0, 1)]
    public float FogCorruptionRestoreRate;
}

[CreateAssetMenu(fileName = "Gameplay Settings", menuName = "Settings/Gameplay Settings")]
public class GameplaySettings : ScriptableObject
{
    public PlayerSettings PlayerSettings;

    [Tooltip("The time in seconds it takes for the fog to have fully taken over")]
    public float FogShrinkDuration;

    [Tooltip("Vertical Axis: The amount of time in seconds it takes for monsters to spawn, Horizontal Axis: Time elapsed")]
    public AnimationCurve SpawnCurve;

    public int MaxEnemies;
}
