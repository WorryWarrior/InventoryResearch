using Cysharp.Threading.Tasks;
using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServicePlayerPrefs : ISaveLoadService
    {
        private const string InventoryKey = "Inventory";

        private readonly IPersistentDataService _persistentDataService;

        public SaveLoadServicePlayerPrefs(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void SaveInventory()
        {
            string inventoryStateJson = SerializeObject(_persistentDataService.Inventory);
            PlayerPrefs.SetString(InventoryKey, inventoryStateJson);
        }

        public UniTask<InventoryData> LoadInventory()
        {
            InventoryData inventoryState = DeserializeObject<InventoryData>(PlayerPrefs.GetString(InventoryKey));
            return UniTask.FromResult(inventoryState);
        }
    }
}