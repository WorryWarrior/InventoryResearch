using Content.Data;
using Content.Gameplay.Items;

namespace Content.Infrastructure.Services.Inventory
{
    public delegate void OnInventorySlotUpdatedEventHandler(int inventorySlotIndex);

    public interface IInventoryService
    {
        public event OnInventorySlotUpdatedEventHandler OnInventorySlotUpdated;
        public int InventorySize { get; }

        public bool TryAddInventoryItem(InventoryItem inventoryItem);
        public void DeleteInventoryItem(int itemSlotIndex);
        public bool TryGetInventoryItem(int itemSlotIndex, out ItemSlotData itemSlotData);
        public void SwapInventoryItems(int firstItemIndex, int secondItemIndex);
    }
}