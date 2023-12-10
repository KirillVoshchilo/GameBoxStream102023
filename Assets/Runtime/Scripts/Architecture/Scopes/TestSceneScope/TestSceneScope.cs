using App.Architecture.AppData;
using App.Content.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using App.Content;

public class TestSceneScope : LifetimeScope
{
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private CamerasStorage _camerasStorage;
    [SerializeField] private Configuration _configuration;

    protected override void Configure(IContainerBuilder builder)
    {
        _configuration.Construct();
        builder.RegisterComponent(_configuration);
        builder.RegisterComponent(_playerEntity);
        builder.RegisterComponent(_camerasStorage);
    }
}
