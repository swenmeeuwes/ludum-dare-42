using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenjectBinding), typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }
}
