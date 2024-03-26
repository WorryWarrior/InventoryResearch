using Content.Data;
using Content.Gameplay.Items;
using Content.UI.Windows;
using RSG;
using TMPro;
using UnityEngine;

namespace Content.Menu.Inventory.Popups
{
    public class ArmourPopup : ItemPopupBase
    {
        [SerializeField] private TextMeshProUGUI defenceValueText = null;

        public override Promise<WindowCloseIntent> Show<T>(T data, string titleText = "")
        {
            PopupData = data as InventoryPopupData;
            InventoryItem = PopupData!.InventorySlotData.InventoryItem;

            defenceValueText.text = $"{+((DefenceItem)InventoryItem).Defence}";

            return base.Show(data, titleText);
        }
    }
}