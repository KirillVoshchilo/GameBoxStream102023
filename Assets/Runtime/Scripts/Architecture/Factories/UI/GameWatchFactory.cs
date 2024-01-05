using App.Content.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class GameWatchFactory : ASceneObjectFactory<GameWatchPresenter>
    {
        private const string PREFAB = "Prefabs/UI/GameWatch";

        private readonly LevelTimer _levelTimer;
        private readonly LevelsController _levelsController;

        [Inject]
        public GameWatchFactory(LevelTimer levelTimer,
            LevelsController levelsController)
        {
            _levelTimer = levelTimer;
            _levelsController = levelsController;
        }

        public override GameWatchPresenter Create()
        {
            GameWatchPresenter prefab = Resources.Load<GameWatchPresenter>(PREFAB);
            GameWatchPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_levelTimer, _levelsController);
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(GameWatchPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.Destruct();
            Object.Destroy(obj.gameObject);
        }
    }
}