using System;
using Content.Data;
using Content.Infrastructure.Services.Inventory;
using Content.Infrastructure.Services.Logging;
using Content.Items;
using Content.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Menu
{
    public class InventoryHUDController : MonoBehaviour
    {
        [SerializeField] private InventoryHUDView inventoryHUDView = null;
        [SerializeField] private WindowBase armourPopup = null;

        private IInventoryService _inventoryService;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(
            IInventoryService inventoryService,
            ILoggingService loggingService
        )
        {
            _inventoryService = inventoryService;
            _loggingService = loggingService;
        }

        public async UniTask Initialize()
        {
            _inventoryService.OnInventorySlotUpdated += index => RefreshInventorySlot(inventoryHUDView.GetInventorySlot(index));

            inventoryHUDView.OnInventorySlotsSwapRequested += SwapInventorySlots;

            await inventoryHUDView.CreateInventorySlots(_inventoryService.InventorySize);
            RefreshAllInventorySlots();
        }

        public void ShowItemPopup(int inventorySlotIndex)
        {
            if (_inventoryService.TryGetInventoryItem(inventorySlotIndex, out ItemSlotData itemSlotData))
            {
                switch (itemSlotData.Item.ItemType)
                {
                    case ItemType.Body:
                    case ItemType.Head:
                        ShowArmourPopup(inventorySlotIndex, itemSlotData);
                        return;
                    case ItemType.Bullet:
                        return;
                    case ItemType.Potion:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void RefreshInventorySlot(InventorySlotController inventorySlotController)
        {
            if (_inventoryService.TryGetInventoryItem(inventorySlotController.SlotIndex, out ItemSlotData itemSlot))
            {
                inventorySlotController.UpdateSlotItemQuantity(itemSlot.Quantity);
            }
            else
            {
                inventorySlotController.SetSlotEmpty();
            }
        }

        private void RefreshAllInventorySlots()
        {
            for (int i = 0; i < _inventoryService.InventorySize; i++)
            {
                RefreshInventorySlot(inventoryHUDView.GetInventorySlot(i));
            }
        }

        private void SwapInventorySlots(int firstSlotIndex, int secondSlotIndex) =>
            _inventoryService.SwapInventoryItems(firstSlotIndex, secondSlotIndex);

        private void ShowArmourPopup(int inventorySlotIndex, ItemSlotData inventorySlotData)
        {
            armourPopup.Show(inventorySlotData, inventorySlotData.Item.Name)
                .Then(intent =>
                {
                    //_loggingService.LogMessage(intent.ToString());

                    switch (intent)
                    {
                        case WindowCloseIntent.Accepted:
                            break;
                        case WindowCloseIntent.Rejected:
                            _inventoryService.DeleteInventoryItem(inventorySlotIndex);
                            break;
                        case WindowCloseIntent.Cancelled:
                            break;
                    }
                });
        }
    }
}