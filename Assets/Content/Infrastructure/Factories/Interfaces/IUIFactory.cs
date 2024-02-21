using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        UniTask WarmUp();
        void CleanUp();
        UniTask CreateUIRoot();
       // UniTask<InventoryHUDController> CreateInventoryHud();
    }
}