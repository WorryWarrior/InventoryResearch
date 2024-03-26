using System;
using Content.Gameplay.Items;

namespace Content.Data
{
    [Serializable]
    public class EquipmentData
    {
        public InventoryItem HeadItem { get; set; }
        public InventoryItem BodyItem { get; set; }
    }
}