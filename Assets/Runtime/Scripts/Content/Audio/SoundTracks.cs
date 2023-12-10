using System;
using UnityEngine;

namespace App.Content.Audio
{
    [Serializable]
    public sealed class SoundTracks
    {
        [SerializeField] private AudioSource _button;
        [SerializeField] private AudioSource _cutSceneChanging;
        [SerializeField] private AudioSource _closeInventory;
        [SerializeField] private AudioSource _changingCells;
        [SerializeField] private AudioSource _itemTransferSound;
        [SerializeField] private AudioSource _openFevoniaInterface;
        [SerializeField] private AudioSource _openGregoryInterface;

        public AudioSource Button => _button;
        public AudioSource CutSceneChanging => _cutSceneChanging;
        public AudioSource CloseInventory => _closeInventory;
        public AudioSource ChangingCells => _changingCells;
        public AudioSource ItemTransfer => _itemTransferSound;
        public AudioSource OpenFevoniaInterface => _openFevoniaInterface;
        public AudioSource OpenGregoryInterface => _openGregoryInterface;
    }
}