using Content.Data;
using Content.Items;

namespace Content.Infrastructure.Services.Inventory
{
    public delegate void OnInventorySlotUpdatedEventHandler(int inventorySlotIndex);

    public interface IInventoryService
    {
        public event OnInventorySlotUpdatedEventHandler OnInventorySlotUpdated;
        public int InventorySize { get; }

        public bool TryAddInventoryItem(ItemBase item);
        public void DeleteInventoryItem(int itemSlotIndex);
        public bool TryGetInventoryItem(int itemSlotIndex, out ItemSlotData itemSlotData);
        public void SwapInventoryItems(int firstItemIndex, int secondItemIndex);
    }
}