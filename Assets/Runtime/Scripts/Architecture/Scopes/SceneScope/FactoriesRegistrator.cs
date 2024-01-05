using App.Architecture.Factories.UI;
using App.Content.Bonfire;
using VContainer;

namespace App.Architecture.Scopes
{
    public sealed class FactoriesRegistrator
    {
        public FactoriesRegistrator(IContainerBuilder builder)
        {
            builder.Register<BonfireFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<FevroniaMenuFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<FreezeEffectFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<GameWatchFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<GrigoryMenuFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<InteractionIconFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<InventoryMenuFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<MainMenuFactory>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<PauseMenuFactory>(Lifetime.Singleton)
                .AsSelf();
        }
    }
}