using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrCreateNew();

            _stateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {

        }

        private async void LoadProgressOrCreateNew()
        {
            _persistentDataService.Inventory = await _saveLoadService.LoadInventory() ?? CreateNewInventory();
        }

        private InventoryData CreateNewInventory() => new()
        {
            ItemSlots = new ItemSlotData[]
            {
                new ItemSlotData()
                {
                    ID = "Uninitialized",
                    Quantity = -1
                }
            }
        };
    }
}