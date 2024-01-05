using App.Architecture.AppInput;
using App.Content.Bonfire;
using App.Content.Player;
using App.Content.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class PauseMenuFactory : ASceneObjectFactory<PauseMenuPresenter>
    {
        private const string PREFAB = "Prefabs/UI/PauseMenu";

        private readonly PlayerEntity _playerEntity;
        private readonly IAppInputSystem _appInputSystem;
        private readonly LevelsController _levelsController;
        private readonly BonfireFactory _bonfireFactory;
        
        [Inject]
        public PauseMenuFactory(PlayerEntity playerEntity,
            IAppInputSystem appInputSystem,
            LevelsController levelsController,
            BonfireFactory bonfireFactory)
        {
            _playerEntity = playerEntity;
            _appInputSystem = appInputSystem;
            _levelsController = levelsController;
            _bonfireFactory = bonfireFactory;
        }


        public override PauseMenuPresenter Create()
        {
            PauseMenuPresenter prefab = Resources.Load<PauseMenuPresenter>(PREFAB);
            PauseMenuPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_appInputSystem,
                _playerEntity,
                _bonfireFactory,
                _levelsController);
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(PauseMenuPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.Destruct();
            Object.Destroy(obj.gameObject);
        }
    }
}