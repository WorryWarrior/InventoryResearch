using Content.Data;
using Content.Gameplay.Items;
using Content.UI.Windows;
using RSG;
using TMPro;
using UnityEngine;

namespace Content.Menu.Inventory.Popups
{
    public class AmmoPopup : ItemPopupBase
    {
        [SerializeField] private TextMeshProUGUI damageValueText = null;

        public override Promise<WindowCloseIntent> Show<T>(T data, string titleText = "")
        {
            PopupData = data as InventoryPopupData;
            InventoryItem = PopupData!.InventorySlotData.InventoryItem;

            damageValueText.text = $"{((AmmoItem)InventoryItem).Damage}";

            return base.Show(data, titleText);
        }
    }
}