using System.Collections.Generic;
using Content.Infrastructure.Factories.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Menu
{
    public delegate void DoubleInventorySlotIndexEventHandler(int firstSlotIndex, int secondSlotIndex);

    public class InventoryHUDView : MonoBehaviour
    {
        [SerializeField] private InventoryHUDController inventoryHUDController = null;
        [SerializeField] private RectTransform inventorySlotContainer = null;

        private IUIFactory _uiFactory;

        public event DoubleInventorySlotIndexEventHandler OnInventorySlotsSwapRequested;

        private readonly List<InventorySlotController> _inventorySlotControllers = new();

        private int _dragStartSlotIndex;
        private int _dragEndSlotIndex = -1;

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
                inventorySlot.OnInventorySlotClicked += clickedSlotIndex => inventoryHUDController.ShowItemPopup(clickedSlotIndex);
                inventorySlot.OnInventorySlotDragStarted += draggedSlot => _dragStartSlotIndex = draggedSlot;

                inventorySlot.OnInventorySlotDragEnded += () =>
                {
                    if (_dragEndSlotIndex != -1 && _dragStartSlotIndex != _dragEndSlotIndex)
                    {
                        OnInventorySlotsSwapRequested?.Invoke(_dragStartSlotIndex, _dragEndSlotIndex);
                    }
                };

                inventorySlot.OnInventorySlotHoverStarted += hoveredSlot => _dragEndSlotIndex = hoveredSlot;
                inventorySlot.OnInventorySlotHoverEnded += () => _dragEndSlotIndex = -1;

                inventorySlot.Initialize(i);
                inventorySlot.transform.SetParent(inventorySlotContainer);
                _inventorySlotControllers.Add(inventorySlot);
            }
        }

        public InventorySlotController GetInventorySlot(int inventorySlotIndex) =>
            _inventorySlotControllers[inventorySlotIndex];
    }
}