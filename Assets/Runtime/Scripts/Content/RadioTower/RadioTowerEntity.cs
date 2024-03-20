using App.Content;
using App.Content.Helicopter;
using App.Content.Player;
using App.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.RadioTower
{
    public class RadioTowerEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private RadioTowerData _data;

        private SoundHandler _soundHandler;
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;

                _isEnable = value;

                if (value)
                    _soundHandler.IsEnable = true;
                else _soundHandler.IsEnable = false;
            }
        }

        [Inject]
        public void Construct(FinishGameController finishController,
            UIController uiController,
            LevelsController levelsController)
        {
            _data.UiController = uiController;
            _data.LevelsController = levelsController;
            _data.FinishGameController = finishController;

            _soundHandler = new SoundHandler(_data);
        }

        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(Activation))
                return _data.Activation as T;

            return null;
        }
    }
}