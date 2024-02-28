using System.IO;
using Content.Data;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServiceJsonFile : ISaveLoadService
    {
        private const string InventorySaveFileName = "Inventory.json";

        private readonly IPersistentDataService _persistentDataService;
        private readonly ILoggingService _loggingService;

        public SaveLoadServiceJsonFile(
            IPersistentDataService persistentDataService,
            ILoggingService loggingService)
        {
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public void SaveInventory()
        {
            string inventoryStateJson = SerializeObject(_persistentDataService.Inventory, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            using StreamWriter sw = File.CreateText(GetInventorySaveFilePath());
            sw.WriteLine(inventoryStateJson);

            _loggingService.LogMessage($"Saved inventory to file at {GetInventorySaveFilePath()}", this);
        }

        public UniTask<InventoryData> LoadInventory()
        {
            if (!File.Exists(GetInventorySaveFilePath()))
                return UniTask.FromResult((InventoryData)null);

            InventoryData inventoryState = DeserializeObject<InventoryData>(File.ReadAllText(GetInventorySaveFilePath()), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            _loggingService.LogMessage($"Loaded inventory from file at {GetInventorySaveFilePath()}", this);

            return UniTask.FromResult(inventoryState);
        }

        private string GetInventorySaveFilePath() =>
            $"{UnityEngine.Application.persistentDataPath}{Path.DirectorySeparatorChar}{InventorySaveFileName}";
    }
}