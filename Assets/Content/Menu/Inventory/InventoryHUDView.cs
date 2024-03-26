using System.Collections.Generic;
using Content.Infrastructure.Factories.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Menu.Inventory
{
    public delegate void DoubleInventorySlotIndexEventHandler(int firstSlotIndex, int secondSlotIndex);

    public class InventoryHUDView : MonoBehaviour
    {
        private const int DragSlotIndexUninitializedValue = -1;

        [SerializeField] private InventoryHUDController inventoryHUDController = null;
        [SerializeField] private RectTransform inventorySlotContainer = null;

        private IUIFactory _uiFactory;
        private readonly List<InventorySlotController> _inventorySlotControllers = new();
        private InventoryDragPreviewController _dragPreviewController;
        private int _dragStartSlotIndex;
        private int _dragEndSlotIndex = DragSlotIndexUninitializedValue;

        public event DoubleInventorySlotIndexEventHandler OnInventorySlotsSwapRequested;

        [Inject]
        private void Construct(
            IUIFactory uiFactory
        )
        {
            _uiFactory = uiFactory;
        }

        public async UniTask CreateInventorySlots(int inventorySlotCount)
        {
            for (int i = 0; i < inventorySlotCount; i++)
            {
                InventorySlotController inventorySlot = await _uiFactory.CreateInventorySlot();

                inventorySlot.OnInventorySlotClicked += async clickedSlotIndex =>
                    await inventoryHUDController.ShowItemPopup(clickedSlotIndex);

                inventorySlot.OnInventorySlotDragStarted += draggedSlotIndex =>
                {
                    _dragPreviewController.SetPreviewSprite(GetInventorySlot(draggedSlotIndex).SlotItemSprite);
                    _dragPreviewController.TogglePreview(true);

                    _dragStartSlotIndex = draggedSlotIndex;
                };

                inventorySlot.OnInventorySlotDragEnded += () =>
                {
                    _dragPreviewController.TogglePreview(false);

                    if (_dragEndSlotIndex != DragSlotIndexUninitializedValue && _dragStartSlotIndex != _dragEndSlotIndex &&
                        !(GetInventorySlot(_dragStartSlotIndex).IsEmpty && GetInventorySlot(_dragEndSlotIndex).IsEmpty))
                    {
                        OnInventorySlotsSwapRequested?.Invoke(_dragStartSlotIndex, _dragEndSlotIndex);
                    }
                };

                inventorySlot.OnInventorySlotHoverStarted += hoveredSlot => _dragEndSlotIndex = hoveredSlot;
                inventorySlot.OnInventorySlotHoverEnded += () => _dragEndSlotIndex = DragSlotIndexUninitializedValue;

                inventorySlot.Initialize(i);
                inventorySlot.transform.SetParent(inventorySlotContainer);
                _inventorySlotControllers.Add(inventorySlot);
            }
        }

        public async UniTask CreateInventoryDragPreview()
        {
            _dragPreviewController = await _uiFactory.CreateInventoryDragPreview();
            _dragPreviewController.TogglePreview(false);
            _dragPreviewController.transform.SetParent(transform);
        }

        public InventorySlotController GetInventorySlot(int inventorySlotIndex) =>
            _inventorySlotControllers[inventorySlotIndex];
    }
}