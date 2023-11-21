using App.Architecture.AppInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Architecture.Scopes
{
    public sealed class RootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<AppInputSystem>(Lifetime.Singleton)
                .As<IAppInputSystem>();
            Debug.Log("Сконфигурировал RootScope.");
        }
    }
}