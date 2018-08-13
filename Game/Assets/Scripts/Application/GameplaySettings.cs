using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    [Tooltip("Rate of fire in seconds")]
    public float FireRate;

    public float BulletSpeed;
    public float BulletDamage;

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
    [Tooltip("Vertical Axis: The amount of monsters that will be created per spawn, Horizontal Axis: Time elapsed")]
    public AnimationCurve SpawnAmountCurve;
    [Tooltip("Vertical Axis: Monsters will have their health multiplied by this amount, Horizontal Axis: Time elapsed")]
    public AnimationCurve SpawnHealthModifier;

    public int MaxEnemies;

    [Range(0, 1)] public float PowerupDropChance;
    public float PowerupStatIncrease; // increment when picked up todo: for individual powerup types
}
