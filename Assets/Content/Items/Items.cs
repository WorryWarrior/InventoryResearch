namespace Content.Items
{
    public class BodyItem : ItemBase
    {
        public override ItemType ItemType => ItemType.Body;
        public int Defence { get; set; }
    }

    public class HeadItem : ItemBase
    {
        public override ItemType ItemType => ItemType.Head;
        public int Defence { get; set; }
    }


}