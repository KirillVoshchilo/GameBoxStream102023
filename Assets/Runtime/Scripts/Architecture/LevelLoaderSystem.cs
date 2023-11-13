using UnityEngine.SceneManagement;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using VContainer;
using VContainer.Unity;
using App.Logic;

namespace App.Architecture
{
    public sealed class LevelLoaderSystem
    {
        public const string MAIN_SCENE = "StartScene";
        public const string FIRST_LEVEL = "FIrstScene";

        private readonly LifetimeScope _container;
        private LevelStorage _currentLoadedLevel;

        [Inject]
        public LevelLoaderSystem(LifetimeScope lifetimeScope)
            => _container = lifetimeScope;

        public async UniTask LoadScene(string scene, Action<LevelStorage> onComplete)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            await UniTask.Yield();
            _currentLoadedLevel = GameObject.FindAnyObjectByType<LevelStorage>();
            _currentLoadedLevel.Construct(_container);
            onComplete?.Invoke(_currentLoadedLevel);
        }
        public void UnloadScene(string scene)
        {
            if (_currentLoadedLevel != null)
                _currentLoadedLevel.Destruct();
            _currentLoadedLevel = null;
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}