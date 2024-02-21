using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public interface IPersistentDataService
    {
        InventoryData Inventory { get; set; }
    }
}