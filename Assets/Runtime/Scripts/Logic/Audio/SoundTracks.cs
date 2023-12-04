using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class SoundTracks
    {
        [SerializeField] private AudioSource _button;
        [SerializeField] private AudioSource _cutSceneChanging;
        [SerializeField] private AudioSource _oneStep;
        [SerializeField] private AudioSource _cough;
        [SerializeField] private AudioSource _chop;
        [SerializeField] private AudioSource _fallingTree;
        [SerializeField] private AudioSource _openInterface;
        [SerializeField] private AudioSource _closeInventory;
        [SerializeField] private AudioSource _changingCells;
        [SerializeField] private AudioSource _itemTransferSound;
        [SerializeField] private AudioSource _helicopter;
        [SerializeField] private AudioSource _openFevoniaInterface;
        [SerializeField] private AudioSource _openGregoryInterface;

        public AudioSource Button { get => _button; set => _button = value; }
        public AudioSource CutSceneChanging { get => _cutSceneChanging; set => _cutSceneChanging = value; }
        public AudioSource OneStep { get => _oneStep; set => _oneStep = value; }
        public AudioSource Cough { get => _cough; set => _cough = value; }
        public AudioSource Chop { get => _chop; set => _chop = value; }
        public AudioSource FallingTree { get => _fallingTree; set => _fallingTree = value; }
        public AudioSource OpenInterface { get => _openInterface; set => _openInterface = value; }
        public AudioSource CloseInventory { get => _closeInventory; set => _closeInventory = value; }
        public AudioSource ChangingCells { get => _changingCells; set => _changingCells = value; }
        public AudioSource ItemTransfer { get => _itemTransferSound; set => _itemTransferSound = value; }
        public AudioSource Helicopter { get => _helicopter; set => _helicopter = value; }
        public AudioSource OpenFevoniaInterface { get => _openFevoniaInterface; set => _openFevoniaInterface = value; }
        public AudioSource OpenGregoryInterface { get => _openGregoryInterface; set => _openGregoryInterface = value; }
    }
}