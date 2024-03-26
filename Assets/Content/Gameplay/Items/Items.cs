namespace Content.Gameplay.Items
{
    public abstract class DefenceItem : InventoryItem
    {
        public int Defence { get; set; }
    }
    public class BodyItem : DefenceItem
    {
        public override ItemType ItemType => ItemType.Body;
    }

    public class HeadItem : DefenceItem
    {
        public override ItemType ItemType => ItemType.Head;
    }

    public class AmmoItem : InventoryItem
    {
        public int Damage { get; set; }
        public override ItemType ItemType => ItemType.Ammo;
    }
}