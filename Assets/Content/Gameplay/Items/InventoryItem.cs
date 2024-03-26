using System;

namespace Content.Gameplay.Items
{
    [Serializable]
    public abstract class InventoryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int MaxStackQuantity { get; set; }
        public abstract ItemType ItemType { get; }
    }
}