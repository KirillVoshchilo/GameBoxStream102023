﻿using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class CycleTracks
    {
        [SerializeField] private AudioSource _mainMenuMusic;
        [SerializeField] private AudioSource _cutSceneMusic;
        [SerializeField] private AudioSource _locationSoundtrack;
        [SerializeField] private AudioSource _burningBonfire;
        [SerializeField] private AudioSource _finalMusic_4;
        [SerializeField] private AudioSource _finalMusic_1;
        [SerializeField] private AudioSource _finalMusic_2;
        [SerializeField] private AudioSource _finalMusic_3;
        [SerializeField] private AudioSource _birdsSounds;
        [SerializeField] private AudioSource _windmillSounds;

        public AudioSource MainMenuMusic => _mainMenuMusic;
        public AudioSource CutSceneMusic => _cutSceneMusic;
        public AudioSource LocationSoundtrack => _locationSoundtrack;
        public AudioSource BurningBonfire => _burningBonfire;
        public AudioSource FinalMusic_4 => _finalMusic_4;
        public AudioSource FinalMusic_1 => _finalMusic_1;
        public AudioSource FinalMusic_2 => _finalMusic_2;
        public AudioSource FinalMusic_3 => _finalMusic_3;
        public AudioSource BirdsSounds => _birdsSounds;
        public AudioSource WindmillSounds => _windmillSounds;
    }
}