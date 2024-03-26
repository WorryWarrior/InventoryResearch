using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        public InventoryData Inventory { get; set; }
        public EquipmentData Equipment { get; set; }
    }
}