using System;
using Content.Data;
using Content.Gameplay.Items;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Services.Inventory;
using Content.Infrastructure.Services.Logging;
using Content.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Menu.Inventory
{
    public class InventoryHUDController : MonoBehaviour
    {
        [SerializeField] private InventoryHUDView inventoryHUDView = null;
        [SerializeField] private WindowBase armourPopup = null;
        [SerializeField] private WindowBase ammoPopup = null;

        private IInventoryService _inventoryService;
        private ILoggingService _loggingService;

        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(
            IInventoryService inventoryService,
            IAssetProvider assetProvider,
            ILoggingService loggingService
        )
        {
            _inventoryService = inventoryService;
            _loggingService = loggingService;
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            // Warm up sprite provider
            _inventoryService.OnInventorySlotUpdated += async index => await RefreshInventorySlot(index);

            inventoryHUDView.OnInventorySlotsSwapRequested += SwapInventorySlots;

            await inventoryHUDView.CreateInventorySlots(_inventoryService.InventorySize);
            await inventoryHUDView.CreateInventoryDragPreview();

            RefreshAllInventorySlots();
        }

        public async UniTask ShowItemPopup(int inventorySlotIndex)
        {
            if (_inventoryService.TryGetInventoryItem(inventorySlotIndex, out ItemSlotData itemSlot))
            {
                InventoryPopupData inventoryPopupData = new InventoryPopupData
                {
                    InventorySlotData = itemSlot,
                    InventorySlotIcon = await _assetProvider.Load<Sprite>(itemSlot.InventoryItem.Id)
                };

                _loggingService.LogMessage(itemSlot.InventoryItem.ItemType.ToString());

                switch (itemSlot.InventoryItem.ItemType)
                {
                    case ItemType.Body or ItemType.Head:
                        ShowPopup(armourPopup, inventorySlotIndex, inventoryPopupData, () =>
                        {
                            _loggingService.LogMessage("Defence item accepted");
                        });
                        return;
                    case ItemType.Ammo:
                        ShowPopup(ammoPopup, inventorySlotIndex, inventoryPopupData, () =>
                        {
                            _loggingService.LogMessage("Ammo item accepted");
                        });
                        return;
                    case ItemType.Heal:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async UniTask RefreshInventorySlot(int inventorySlotIndex)
        {
            InventorySlotController inventorySlotController = inventoryHUDView.GetInventorySlot(inventorySlotIndex);

            if (_inventoryService.TryGetInventoryItem(inventorySlotController.SlotIndex, out ItemSlotData itemSlot))
            {
                Sprite itemSprite = await _assetProvider.Load<Sprite>(itemSlot.InventoryItem.Id);

                inventorySlotController.UpdateSlotItemIcon(itemSprite);
                inventorySlotController.UpdateSlotItemQuantity(itemSlot.Quantity);
            }
            else
            {
                inventorySlotController.SetSlotEmpty();
            }
        }

        private async void RefreshAllInventorySlots()
        {
            for (int i = 0; i < _inventoryService.InventorySize; i++)
            {
                await RefreshInventorySlot(i);
            }
        }

        private void SwapInventorySlots(int firstSlotIndex, int secondSlotIndex) =>
            _inventoryService.SwapInventoryItems(firstSlotIndex, secondSlotIndex);

        private void ShowPopup(WindowBase popup, int inventorySlotIndex, InventoryPopupData inventoryPopupData,
            Action onPopupAccepted = null)
        {
            popup.Show(inventoryPopupData, inventoryPopupData.InventorySlotData.InventoryItem.Name)
                .Then(intent =>
                {
                    switch (intent)
                    {
                        case WindowCloseIntent.Accepted:
                            onPopupAccepted?.Invoke();
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