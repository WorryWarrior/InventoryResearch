using System;

namespace Content.Data
{
    [Serializable]
    public class InventoryData
    {
        public ItemSlotData[] ItemSlots { get; set; }
    }
}