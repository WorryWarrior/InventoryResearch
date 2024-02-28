using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Content.Menu
{
    public class InventorySlotController : MonoBehaviour,
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button clickButton = null;
        [SerializeField] private TextMeshProUGUI quantityText = null;

        public int SlotIndex { get; private set; }

        public event Action<int> OnInventorySlotClicked;
        public event Action<int> OnInventorySlotDragStarted;
        public event Action OnInventorySlotDragEnded;

        public event Action<int> OnInventorySlotHoverStarted;
        public event Action OnInventorySlotHoverEnded;

        public void Initialize(int slotIndex)
        {
            clickButton.onClick.AddListener(() => OnInventorySlotClicked?.Invoke(SlotIndex));
            SlotIndex = slotIndex;
        }

        public void UpdateSlotItemQuantity(int value)
        {
            string textValue = value > 1 ? value.ToString() : string.Empty;
            quantityText.text = textValue;
        }

        public void SetSlotEmpty()
        {
            UpdateSlotItemQuantity(0);
        }

        public void OnBeginDrag(PointerEventData eventData) => OnInventorySlotDragStarted?.Invoke(SlotIndex);

        public void OnEndDrag(PointerEventData eventData) => OnInventorySlotDragEnded?.Invoke();

        public void OnPointerEnter(PointerEventData eventData) => OnInventorySlotHoverStarted?.Invoke(SlotIndex);

        public void OnPointerExit(PointerEventData eventData) => OnInventorySlotHoverEnded?.Invoke();

        public void OnDrag(PointerEventData eventData) { }
    }
}