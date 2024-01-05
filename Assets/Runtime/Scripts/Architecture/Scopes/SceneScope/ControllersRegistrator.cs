using App.Logic;
using VContainer;

namespace App.Architecture.Scopes
{
    public sealed class ControllersRegistrator
    {
        public ControllersRegistrator(IContainerBuilder builder)
        {
            builder.Register<EndLevelController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<UIController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<NewGameController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<LevelsController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<AllSnowController>(Lifetime.Singleton)
                .AsSelf();
            builder.Register<FinishGameController>(Lifetime.Singleton)
                .AsSelf();
        }

        public void Resolver(IObjectResolver resolver)
        {
            EndLevelController endLevelController = resolver.Resolve<EndLevelController>();
            FinishGameController finishController = resolver.Resolve<FinishGameController>();
            finishController.Construct();
        }
    }
}