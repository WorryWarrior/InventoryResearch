using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Content.Menu.Inventory
{
    public class InventorySlotController : MonoBehaviour,
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        private const int SlotQuantityEmpty = -1;

        [SerializeField] private Button clickButton = null;
        [SerializeField] private Image itemIcon = null;
        [SerializeField] private TextMeshProUGUI quantityText = null;

        public int SlotIndex { get; private set; }
        public bool IsEmpty { get; private set; }
        public Sprite SlotItemSprite => itemIcon.sprite;

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

            IsEmpty = value == SlotQuantityEmpty;
        }

        public void UpdateSlotItemIcon(Sprite value)
        {
            itemIcon.sprite = value;
            itemIcon.enabled = value != null;
        }

        public void SetSlotEmpty()
        {
            UpdateSlotItemIcon(null);
            UpdateSlotItemQuantity(SlotQuantityEmpty);
        }

        public void OnBeginDrag(PointerEventData eventData) => OnInventorySlotDragStarted?.Invoke(SlotIndex);
        public void OnEndDrag(PointerEventData eventData) => OnInventorySlotDragEnded?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => OnInventorySlotHoverStarted?.Invoke(SlotIndex);
        public void OnPointerExit(PointerEventData eventData) => OnInventorySlotHoverEnded?.Invoke();
        public void OnDrag(PointerEventData eventData) { }
    }
}