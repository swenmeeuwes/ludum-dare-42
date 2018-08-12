using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpAfter : MonoBehaviour
{
    [SerializeField] private float _seconds = 1;

    private void Start()
    {
        Invoke("Clean", _seconds);
    }

    private void Clean()
    {
        Destroy(gameObject);
    }
}
