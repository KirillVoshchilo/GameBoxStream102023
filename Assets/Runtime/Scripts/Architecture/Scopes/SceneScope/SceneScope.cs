using App.Architecture.AppData;
using App.Content;
using App.Content.Audio;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Architecture.Scopes
{
    public sealed class SceneScope : LifetimeScope
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private CamerasStorage _camerasStorage;
        [SerializeField] private Configuration _configuration;
        [SerializeField] private FallingSnow _fallingSnow;
        [SerializeField] private AudioStorage _audioStorage;
        private ControllersRegistrator _controllerRegistrator;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_configuration);
            builder.RegisterComponent(_audioStorage);
            builder.RegisterComponent(_playerEntity);
            builder.RegisterComponent(_fallingSnow);
            builder.RegisterComponent(_camerasStorage);
            builder.Register<LevelLoaderSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<VillageTrustSystem>(Lifetime.Singleton)
                .AsSelf();
            _controllerRegistrator = new ControllersRegistrator(builder);
            new FactoriesRegistrator(builder);
            builder.RegisterBuildCallback((container) =>
            {
                _controllerRegistrator.Resolver(container);
                LevelLoaderSystem levelLoaderSystem = container.Resolve<LevelLoaderSystem>();
                levelLoaderSystem.LoadScene(ScenesNames.FIRST_LEVEL)
                .Forget();
                UIController uiController = container.Resolve<UIController>();
                uiController.OpenMainMenu();
            });
        }
    }
}