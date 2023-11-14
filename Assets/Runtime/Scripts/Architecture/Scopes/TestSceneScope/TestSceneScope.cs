using App.Architecture.AppData;
using App.Architecture;
using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
