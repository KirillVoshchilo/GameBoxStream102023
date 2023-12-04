using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class AudioData
    {
        [SerializeField] private CycleTracks _cycleTracks;
        [SerializeField] private SoundTracks _soundTracks;

        public CycleTracks CycleTracks => _cycleTracks;
        public SoundTracks SoundTracks => _soundTracks;
    }
}