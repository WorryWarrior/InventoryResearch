using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Scopes
{
    public class InfrastructureScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureProviders(builder);
            ConfigureServices(builder);
            ConfigureFactories(builder);

            ConfigureStates(builder);
        }

        private void ConfigureProviders(IContainerBuilder builder)
        {
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        }

        private void ConfigureServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, InputService>(Lifetime.Singleton);
            builder.Register<ILoggingService, LoggingService>(Lifetime.Singleton);
            builder.Register<IPersistentDataService, PersistentDataService>(Lifetime.Singleton);
            builder.Register<ISaveLoadService, SaveLoadServiceJsonFile>(Lifetime.Singleton);
        }

        private void ConfigureFactories(IContainerBuilder builder)
        {
            builder.Register<StateFactory>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
        }

        private void ConfigureStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadProgressState>(Lifetime.Singleton);
            builder.Register<LoadMetaState>(Lifetime.Singleton);

            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}