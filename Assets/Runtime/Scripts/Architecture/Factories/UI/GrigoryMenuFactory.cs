using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Audio;
using App.Content.Player;
using App.Content.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class GrigoryMenuFactory : ASceneObjectFactory<GrigoryMenuPresenter>
    {
        private const string PREFAB = "Prefabs/UI/GrigoryMenu";

        private readonly PlayerEntity _playerEntity;
        private readonly VillageTrustSystem _villageTrustSystem;
        private readonly Configuration _configurations;
        private readonly IAppInputSystem _appInputSystem;
        private readonly AudioStorage _audioController;
        private readonly LevelsController _levelsController;

        [Inject]
        public GrigoryMenuFactory(PlayerEntity playerEntity,
            VillageTrustSystem villageTrustSystem,
            Configuration configurations,
            IAppInputSystem appInputSystem,
            AudioStorage audioController,
            LevelsController levelsController)
        {
            _playerEntity = playerEntity;
            _villageTrustSystem = villageTrustSystem;
            _configurations = configurations;
            _appInputSystem = appInputSystem;
            _audioController = audioController;
            _levelsController = levelsController;
        }

        public override GrigoryMenuPresenter Create()
        {
            GrigoryMenuPresenter prefab = Resources.Load<GrigoryMenuPresenter>(PREFAB);
            GrigoryMenuPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_playerEntity,
                _configurations,
                _villageTrustSystem,
                _appInputSystem,
                _audioController,
                _levelsController);
            ConfigureDialoges(instance);
            instance.IsEnable = true;
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(GrigoryMenuPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.IsEnable = false;
            Object.Destroy(obj.gameObject);
        }

        private void ConfigureDialoges(GrigoryMenuPresenter presenter)
        {
            int level = _levelsController.CurrentLevel;
            LevelConfiguration levelConfiguration = _configurations.LevelsConfigurations[level];
            if (level == 0 || level == 1)
            {
                presenter.Dialoge = levelConfiguration.StorageDialogs;
                return;
            }
            float currentTrust = _villageTrustSystem.Trust;
            float targetTrust = _configurations.TrustLevels[level - 1];
            if (currentTrust >= targetTrust)
                presenter.Dialoge = levelConfiguration.StorageDialogeWithTip;
            else presenter.Dialoge = levelConfiguration.StorageDialogs;
        }
    }
}