using Content.Data;
using Content.Gameplay.Items;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;

namespace Content.Infrastructure.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        public InventoryService(
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService
        )
        {
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public event OnInventorySlotUpdatedEventHandler OnInventorySlotUpdated;
        public int InventorySize => _persistentDataService.Inventory.ItemSlots.Length;

        public bool TryGetInventoryItem(int itemSlotIndex, out ItemSlotData itemSlotData)
        {
            itemSlotData = null;

            if (_persistentDataService.Inventory.ItemSlots[itemSlotIndex].InventoryItem == null)
                return false;

            itemSlotData = _persistentDataService.Inventory.ItemSlots[itemSlotIndex];

            return true;
        }

        public bool TryAddInventoryItem(InventoryItem inventoryItem)
        {
            if (TryFindFreeInventorySlot(out int freeSlotIndex))
            {
                OnInventorySlotUpdated?.Invoke(freeSlotIndex);
            }

            return false;
        }

        public void DeleteInventoryItem(int itemSlotIndex)
        {
            ItemSlotData itemSlot = _persistentDataService.Inventory.ItemSlots[itemSlotIndex];
            itemSlot.InventoryItem = null;
            itemSlot.Quantity = -1;

            OnInventorySlotUpdated?.Invoke(itemSlotIndex);
        }

        public void SwapInventoryItems(int firstItemIndex, int secondItemIndex)
        {
            (_persistentDataService.Inventory.ItemSlots[firstItemIndex], _persistentDataService.Inventory.ItemSlots[secondItemIndex]) =
                (_persistentDataService.Inventory.ItemSlots[secondItemIndex], _persistentDataService.Inventory.ItemSlots[firstItemIndex]);

            OnInventorySlotUpdated?.Invoke(firstItemIndex);
            OnInventorySlotUpdated?.Invoke(secondItemIndex);

            _saveLoadService.SaveInventory();
        }

        private bool TryFindFreeInventorySlot(out int freeSlotIndex)
        {
            freeSlotIndex = -1;
            return false;
        }
    }
}