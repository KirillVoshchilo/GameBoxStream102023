using System;
using UnityEngine;

namespace App.Content.Audio
{
    [Serializable]
    public sealed class CycleTracks
    {
        [SerializeField] private AudioSource _mainMenuMusic;
        [SerializeField] private AudioSource _cutSceneMusic;
        [SerializeField] private AudioSource _locationSoundtrack;
        [SerializeField] private AudioSource _finalMusic_4;
        [SerializeField] private AudioSource _finalMusic_1;
        [SerializeField] private AudioSource _finalMusic_2;
        [SerializeField] private AudioSource _finalMusic_3;

        public AudioSource MainMenuMusic => _mainMenuMusic;
        public AudioSource CutSceneMusic => _cutSceneMusic;
        public AudioSource LocationSoundtrack => _locationSoundtrack;
        public AudioSource FinalMusic_4 => _finalMusic_4;
        public AudioSource FinalMusic_1 => _finalMusic_1;
        public AudioSource FinalMusic_2 => _finalMusic_2;
        public AudioSource FinalMusic_3 => _finalMusic_3;
    }
}