using System;
using Content.Gameplay.Items;
using Content.StaticData.Converters;
using Newtonsoft.Json;

namespace Content.Data
{
    [Serializable]
    public class ItemSlotData
    {
        public InventoryItem InventoryItem { get; set; }
        public int Quantity { get; set; } = -1;
    }
}