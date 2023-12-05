using App.Architecture.AppData;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioData _audioData;

    private AudioSource _currentMusic;
    public AudioData AudioData => _audioData;

    public void PlayAudioSource(AudioSource audioSource)
    {
        if (_currentMusic != null)
            _currentMusic.Stop();
        _currentMusic = audioSource;
        _currentMusic.Play();
    }
}
