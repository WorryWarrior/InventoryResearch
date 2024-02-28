using System.Collections.Generic;
using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;
using Content.Items;

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

        private InventoryData CreateNewInventory()
        {
            InventoryData inventoryData = new InventoryData
            {
                ItemSlots = new ItemSlotData[30]
            };

            for (int i = 0; i < inventoryData.ItemSlots.Length; i++)
            {
                inventoryData.ItemSlots[i] = new ItemSlotData
                {
                    Item = GetRandomItem(),
                    Quantity = UnityEngine.Random.Range(1, 5)
                };
            }

            return inventoryData;
        }

        private ItemBase GetRandomItem()
        {
            List<ItemBase> itemVariations = new List<ItemBase>
            {
                new BodyItem
                {
                    ID = 0,
                    Name = "Test Body Item",
                    MaxStackQuantity = 1,
                    Weight = UnityEngine.Random.Range(1, 5),
                    Defence = UnityEngine.Random.Range(1, 10),
                },
                new HeadItem
                {
                    ID = 0,
                    Name = "Test Head Item",
                    MaxStackQuantity = 1,
                    Weight = UnityEngine.Random.Range(1, 5),
                    Defence = UnityEngine.Random.Range(1, 10),
                },
            };

            return itemVariations[UnityEngine.Random.Range(0, itemVariations.Count)];
        }
    }
}