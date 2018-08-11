using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Settings", menuName = "Settings/Gameplay Settings")]
public class GameplaySettings : ScriptableObject
{
    [Tooltip("The time in seconds it takes for the fog to have fully taken over")]
    public float FogShrinkDuration;
}
