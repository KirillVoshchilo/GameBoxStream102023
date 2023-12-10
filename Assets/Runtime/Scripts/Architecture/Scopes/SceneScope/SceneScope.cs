using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content;
using App.Content.Audio;
using App.Content.Player;
using App.Logic;
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
        [SerializeField] private ShopFactory[] _shopFactories;
        [SerializeField] private BonfireFactory _bonfireFactory;
        [SerializeField] private FallingSnow _fallingSnowController;
        [SerializeField] private AudioStorage _audioController;

        protected override void Configure(IContainerBuilder builder)
        {
            _configuration.Construct();
            builder.RegisterComponent(_configuration);
            builder.RegisterComponent(_audioController);
            builder.RegisterComponent(_worldCanvasStorage);
            builder.RegisterComponent(_playerEntity);
            builder.RegisterComponent(_fallingSnowController);
            builder.RegisterComponent(_uiController);
            builder.RegisterComponent(_camerasStorage);
            builder.RegisterComponent(_bonfireFactory);
            builder.Register<LevelLoaderSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<VillageTrustSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<DefeatController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<LevelsController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<AllSnowController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<FinishController>(Lifetime.Singleton)
                .AsSelf();
            builder.RegisterBuildCallback((container) =>
            {
                FinishController finishController = container.Resolve<FinishController>();
                finishController.Construct();
                IAppInputSystem appInputSystem = container.Resolve<IAppInputSystem>();
                foreach (ShopFactory shopFactory in _shopFactories)
                    shopFactory.Construct(appInputSystem, _uiController, _worldCanvasStorage);
            });
        }
    }
}