using UnityEngine.SceneManagement;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using VContainer;
using VContainer.Unity;
using App.Content;

namespace App.Architecture
{
    public sealed class LevelLoaderSystem
    {
        private readonly LifetimeScope _container;
        private LevelStorage _currentLoadedLevel;

        public bool LevelIsLoaded => _currentLoadedLevel != null;
        public LevelStorage CurrentLoadedLevel  => _currentLoadedLevel;

        [Inject]
        public LevelLoaderSystem(LifetimeScope lifetimeScope)
            => _container = lifetimeScope;

        public async UniTask LoadScene(string scene, Action<LevelStorage> onComplete)
        {
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _currentLoadedLevel = GameObject.FindAnyObjectByType<LevelStorage>();
            _currentLoadedLevel.Construct(_container);
            onComplete?.Invoke(_currentLoadedLevel);
        }
        public async UniTask LoadScene(string scene)
        {
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _currentLoadedLevel = GameObject.FindAnyObjectByType<LevelStorage>();
            _currentLoadedLevel.Construct(_container);
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