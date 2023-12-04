using App.Architecture.AppData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioData _audioData;

    public AudioData AudioData  => _audioData;
}
