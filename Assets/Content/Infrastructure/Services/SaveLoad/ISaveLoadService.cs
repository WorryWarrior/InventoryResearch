using Content.Data;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveInventory();
        UniTask<InventoryData> LoadInventory();
    }
}