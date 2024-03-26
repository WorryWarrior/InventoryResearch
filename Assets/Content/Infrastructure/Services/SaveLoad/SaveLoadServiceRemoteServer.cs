using Content.Data;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.StaticData.Converters;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServiceRemoteServer : ISaveLoadService
    {
        private const string InventoryServerURL = "http://localhost:8080/newInventory";

        private readonly IPersistentDataService _persistentDataService;
        private readonly ILoggingService _loggingService;

        public SaveLoadServiceRemoteServer(
            IPersistentDataService persistentDataService,
            ILoggingService loggingService)
        {
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public void SaveInventory()
        {
        }

        public async UniTask<InventoryData> LoadInventory()
        {
            /*UnityWebRequest getRequest = UnityWebRequest.Get(InventoryServerURL);

            if (getRequest.result != UnityWebRequest.Result.Success)
            {
                _loggingService.LogWarning("Could not connect to inventory server");
                return null;
            }

            await getRequest.SendWebRequest();

            string result = getRequest.downloadHandler.text;
            _loggingService.LogMessage(result);

            InventoryData fooo = DeserializeObject<InventoryData>(result);
            return fooo;*/

            UniTaskCompletionSource<string> tcs = new UniTaskCompletionSource<string>();

            using (UnityWebRequest www = UnityWebRequest.Get(InventoryServerURL))
            {
                UnityWebRequestAsyncOperation asyncOp = www.SendWebRequest();

                asyncOp.completed += _ =>
                {
                    if (www.result == UnityWebRequest.Result.ConnectionError ||
                        www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        _loggingService.LogMessage("Could not connect to inventory server");
                        tcs.TrySetResult(string.Empty);
                    }
                    else
                    {
                        string inventoryDataJson = www.downloadHandler.text;
                        _loggingService.LogMessage($"{inventoryDataJson}");
                        tcs.TrySetResult(inventoryDataJson);
                    }
                };

                await tcs.Task;
            }

            InventoryData foo = DeserializeObject<InventoryData>(tcs.GetResult(0), new InventoryItemConverter());
            _loggingService.LogMessage($"{foo.ItemSlots[0].InventoryItem == null}");
            return foo;
        }
    }
}