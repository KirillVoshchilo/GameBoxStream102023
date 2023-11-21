using App.Architecture.AppData;
using App.Architecture.AppInput;
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

        protected override void Configure(IContainerBuilder builder)
        {
            _configuration.Construct();
            builder.RegisterComponent(_configuration);
            builder.RegisterComponent(_worldCanvasStorage);
            builder.RegisterComponent(_playerEntity);
            builder.RegisterComponent(_uiController);
            builder.RegisterComponent(_camerasStorage);
            builder.Register<LevelLoaderSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<VillageTrustSystem>(Lifetime.Singleton)
                .AsSelf();
            builder.RegisterBuildCallback((container) =>
            {
                IAppInputSystem appInputSystem = container.Resolve<IAppInputSystem>();
                foreach (ShopFactory shopFactory in _shopFactories)
                    shopFactory.Construct(appInputSystem, _uiController, _worldCanvasStorage);
            });
            Debug.Log("Сконфигурировал SceneScope.");
        }
    }
}