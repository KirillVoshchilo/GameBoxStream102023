using App.Content.Audio;
using App.Content.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class MainMenuFactory : ASceneObjectFactory<MainMenuPresenter>
    {
        private const string PREFAB = "Prefabs/UI/MainMenu";

        private readonly AudioStorage _audioController;
        private readonly LevelLoaderSystem _levelsLoadeSystem;
        private readonly NewGameController _newGameController;

        [Inject]
        public MainMenuFactory(AudioStorage audioController,
            LevelLoaderSystem levelsLoadeSystem,
            NewGameController newGameController)
        {
            _audioController = audioController;
            _levelsLoadeSystem = levelsLoadeSystem;
            _newGameController = newGameController;
        }

        public override MainMenuPresenter Create()
        {
            MainMenuPresenter prefab = Resources.Load<MainMenuPresenter>(PREFAB);
            MainMenuPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_audioController,
                _levelsLoadeSystem,
                _newGameController);
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(MainMenuPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.Destruct();
            Object.Destroy(obj.gameObject);
        }
    }
}