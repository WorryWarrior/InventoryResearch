using Content.Data;
using Content.Gameplay.Items;
using Content.UI.Windows;
using RSG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Menu.Inventory.Popups
{
    public abstract class ItemPopupBase : ThreeButtonWindow
    {
        [Space(20)]
        [SerializeField] private Image itemPreviewIcon = null;
        [SerializeField] private TextMeshProUGUI weightValueText = null;

        protected InventoryPopupData PopupData;
        protected InventoryItem InventoryItem;

        public override Promise<WindowCloseIntent> Show<T>(T data, string titleText = "")
        {
            itemPreviewIcon.sprite = PopupData!.InventorySlotIcon;
            weightValueText.text = $"{Mathf.Floor(InventoryItem.Weight * PopupData!.InventorySlotData.Quantity)} kg";

            return base.Show(data, titleText);
        }
    }
}