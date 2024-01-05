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
    public sealed class FevroniaMenuFactory : ASceneObjectFactory<FevroniaMenuPresenter>
    {
        private const string PREFAB = "Prefabs/UI/FevroniaCanvas";

        private readonly LevelsController _levelsController;
        private readonly PlayerEntity _playerEntity;
        private readonly VillageTrustSystem _villageTrustSystem;
        private readonly Configuration _configurations;
        private readonly IAppInputSystem _appInputSystem;
        private readonly AudioStorage _audioController;

        [Inject]
        public FevroniaMenuFactory(PlayerEntity playerEntity,
            VillageTrustSystem villageTrustSystem,
            Configuration configurations,
            IAppInputSystem appInputSystem,
            AudioStorage audioController,
            LevelsController levelsController)
        {
            _levelsController = levelsController;
            _playerEntity = playerEntity;
            _villageTrustSystem = villageTrustSystem;
            _configurations = configurations;
            _appInputSystem = appInputSystem;
            _audioController = audioController;
        }

        public override FevroniaMenuPresenter Create()
        {
            FevroniaMenuPresenter prefab = Resources.Load<FevroniaMenuPresenter>(PREFAB);
            FevroniaMenuPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_playerEntity,
                _configurations,
                _villageTrustSystem,
                _appInputSystem,
                _audioController);
            instance.CurrentLevel = _levelsController.CurrentLevel;
            ConfigureDialoges(instance);
            instance.IsEnable = true;
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(FevroniaMenuPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.IsEnable = false;
            Object.Destroy(obj.gameObject);
        }

        private void ConfigureDialoges(FevroniaMenuPresenter presenter)
        {
            int level = _levelsController.CurrentLevel;
            LevelConfiguration levelConfiguration = _configurations.LevelsConfigurations[level];
            presenter.Dialoge = levelConfiguration.ScarecrowDialogs;
        }
    }
}