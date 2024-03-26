using System;
using Content.Gameplay.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Content.StaticData.Converters
{
    public class InventoryItemConverter : JsonConverter<InventoryItem>
    {
        public override InventoryItem ReadJson(JsonReader reader, Type objectType, InventoryItem existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jsonObject = JObject.Load(reader);
            ItemType itemType = (ItemType)jsonObject["ItemType"]!.Value<int>();

            InventoryItem item = itemType switch
            {
                ItemType.Body => new BodyItem(),
                ItemType.Head => new HeadItem(),
                ItemType.Ammo => new AmmoItem(),
                ItemType.Heal => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            serializer.Populate(jsonObject.CreateReader(), item);

            return item;
        }

        public override void WriteJson(JsonWriter writer, InventoryItem value, JsonSerializer serializer) { }
    }
}