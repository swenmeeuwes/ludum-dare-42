using System;
using UnityEngine;

[Serializable]
public class Prefabs
{
    public GameObject ScreenRoot;
    public GameObject Bullet;
    public GameObject HintText;

    [Header("Enemy prefabs")]
    public GameObject SlowEnemy;
    public GameObject FastEnemy;
    public GameObject ShootingEnemy;

    [Header("Powerup prefabs")]
    public GameObject Powerup;
}

[CreateAssetMenu(fileName = "Project Settings", menuName = "Settings/Project Settings")]
public class ProjectSettings : ScriptableObject
{
    public ControllerSettings ControllerSettings;
    public GameplaySettings GameplaySettings;
    public ScreenContext ScreenContext;
    public Prefabs Prefabs;
}
