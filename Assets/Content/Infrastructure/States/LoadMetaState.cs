using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISaveLoadService _saveLoadService;

        public LoadMetaState(
            GameStateMachine stateMachine,
            IUIFactory uiFactory,
            ISceneLoader sceneLoader,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _saveLoadService = saveLoadService;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUp();
            await _sceneLoader.LoadScene(SceneName.Menu, OnMetaSceneLoaded);

            _saveLoadService.SaveInventory();
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }

        private async void OnMetaSceneLoaded(SceneName sceneName)
        {
            await ConstructUIRoot();
            //await ConstructSelectionMenu();
        }

        private async UniTask ConstructUIRoot() => await _uiFactory.CreateUIRoot();

        /*private async Task ConstructSelectionMenu()
        {
            await _uiFactory
                .CreateSelectionMenu()
                .ContinueWith(it => it.Result.Initialize(), TaskScheduler.FromCurrentSynchronizationContext());
        }*/
    }
}