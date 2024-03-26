using Content.Menu;
using Content.Menu.Equipment;
using Content.Menu.Inventory;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        UniTask WarmUp();
        void CleanUp();
        UniTask CreateUIRoot();
        UniTask<InventoryHUDController> CreateInventoryHUD();
        UniTask<InventorySlotController> CreateInventorySlot();
        UniTask<InventoryDragPreviewController> CreateInventoryDragPreview();
        UniTask<EquipmentHUDController> CreateEquipmentHUD();
    }
}