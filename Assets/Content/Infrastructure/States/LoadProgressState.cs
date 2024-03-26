using System.Collections.Generic;
using Content.Data;
using Content.Gameplay.Items;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;
using Cysharp.Threading.Tasks;

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

        public async void Enter()
        {
            await LoadProgressOrCreateNew();

            _stateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {

        }

        private async UniTask LoadProgressOrCreateNew()
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
                InventoryItem randomInventoryItem = GetRandomItem();

                inventoryData.ItemSlots[i] = new ItemSlotData
                {
                    InventoryItem = randomInventoryItem,
                    Quantity = randomInventoryItem == null ? -1 : UnityEngine.Random.Range(1, randomInventoryItem.MaxStackQuantity)
                };
            }

            return inventoryData;
        }

        private InventoryItem GetRandomItem()
        {
            List<InventoryItem> itemVariations = new List<InventoryItem>
            {
                new BodyItem
                {
                    Id = "Item_Body_Jacket",
                    Name = "Jacket",
                    MaxStackQuantity = 1,
                    Weight = 2f,
                    Defence = UnityEngine.Random.Range(1, 2),
                },
                new BodyItem
                {
                    Id = "Item_Body_Vest",
                    Name = "Vest",
                    MaxStackQuantity = 1,
                    Weight = 20f,
                    Defence = UnityEngine.Random.Range(8, 10),
                },
                new HeadItem
                {
                    Id = "Item_Head_Helmet",
                    Name = "Helmet",
                    MaxStackQuantity = 1,
                    Weight = 10f,
                    Defence = UnityEngine.Random.Range(5, 10),
                },
                new HeadItem
                {
                    Id = "Item_Head_Cap",
                    Name = "Cap",
                    MaxStackQuantity = 1,
                    Weight = 1f,
                    Defence = UnityEngine.Random.Range(1, 3),
                },
                new AmmoItem
                {
                    Id = "Item_Ammo_Rifle",
                    Name = "Rifle Ammo",
                    MaxStackQuantity = 50,
                    Weight = 0.1f,
                    Damage = 2
                },
                null
            };

            return itemVariations[UnityEngine.Random.Range(0, itemVariations.Count)];
        }
    }
}