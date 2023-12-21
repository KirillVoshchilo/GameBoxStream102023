using App.Architecture.AppData;
using App.Content;
using App.Content.Audio;
using App.Content.Bonfire;
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
        [SerializeField] private WorldCanvasStorage _worldCanvasStorage;
        [SerializeField] private UIController _uiController;
        [SerializeField] private Configuration _configuration;
        [SerializeField] private BonfireFactory _bonfireFactory;
        [SerializeField] private FallingSnow _fallingSnow;
        [SerializeField] private AudioStorage _audioStorage;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_configuration);
            builder.RegisterComponent(_audioStorage);
            builder.RegisterComponent(_worldCanvasStorage);
            builder.RegisterComponent(_playerEntity);
            builder.RegisterComponent(_fallingSnow);
            builder.RegisterComponent(_uiController);
            builder.RegisterComponent(_camerasStorage);
            builder.RegisterComponent(_bonfireFactory);
            builder.Register<LevelLoaderSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<VillageTrustSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<EndLevelController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<LevelsController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<AllSnowController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<FinishGameController>(Lifetime.Singleton)
                .AsSelf();
            builder.RegisterBuildCallback((container) =>
            {
                FinishGameController finishController = container.Resolve<FinishGameController>();
                finishController.Construct();
                LevelLoaderSystem levelLoaderSystem = container.Resolve<LevelLoaderSystem>();
                levelLoaderSystem.LoadScene(ScenesNames.FIRST_LEVEL)
                .Forget();
            });
        }
    }
}