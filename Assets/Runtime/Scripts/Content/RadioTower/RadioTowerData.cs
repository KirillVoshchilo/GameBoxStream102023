using App.Logic;
using System;
using UnityEngine;

namespace App.Content.RadioTower
{
    [Serializable]
    public sealed class RadioTowerData
    {
        [SerializeField] private AudioSource _audioSource;

        private Activation _activation;
        private FinishGameController _finishGameController;
        private UIController _uiController;
        private LevelsController _levelsController;

        public Activation Activation { get => _activation; set => _activation = value; }
        public AudioSource AudioSource  => _audioSource;
        public FinishGameController FinishGameController { get => _finishGameController; set => _finishGameController = value; }
        public UIController UiController { get => _uiController; set => _uiController = value; }
        public LevelsController LevelsController { get => _levelsController; set => _levelsController = value; }
    }
}