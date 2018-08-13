using System;
using UnityEngine;

[Serializable]
public class Prefabs
{
    public GameObject ScreenRoot;
    public GameObject Bullet;
    public GameObject SlowEnemy;
    public GameObject FastEnemy;
}

[CreateAssetMenu(fileName = "Project Settings", menuName = "Settings/Project Settings")]
public class ProjectSettings : ScriptableObject
{
    public ControllerSettings ControllerSettings;
    public GameplaySettings GameplaySettings;
    public ScreenContext ScreenContext;
    public Prefabs Prefabs;
}
