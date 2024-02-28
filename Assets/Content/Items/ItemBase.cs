namespace Content.Items
{
    [System.Serializable]
    public abstract class ItemBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int MaxStackQuantity { get; set; }
        public abstract ItemType ItemType { get; }
    }
}