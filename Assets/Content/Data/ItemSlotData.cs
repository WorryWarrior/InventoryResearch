using System;
using Content.Items;

namespace Content.Data
{
    [Serializable]
    public class ItemSlotData
    {
        public ItemBase Item { get; set; }
        public int Quantity { get; set; }
    }
}