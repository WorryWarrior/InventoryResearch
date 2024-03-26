using UnityEngine;
using UnityEngine.UI;

namespace Content.Menu.Inventory
{
    public class InventoryDragPreviewController : MonoBehaviour
    {
        [SerializeField] private Image previewImage = null;

        private Canvas _parentCanvas;
        private bool _isActive;

        public void Initialize(Canvas parentCanvas)
        {
            _parentCanvas = parentCanvas;
        }

        private void Update()
        {
            if (!_isActive)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                Input.mousePosition, _parentCanvas.worldCamera,
                out Vector2 movePos);

            transform.position = _parentCanvas.transform.TransformPoint(movePos);
        }

        public void TogglePreview(bool value)
        {
            _isActive = value && previewImage.sprite != null;
            previewImage.enabled = _isActive;
        }

        public void SetPreviewSprite(Sprite value)
        {
            previewImage.sprite = value;
        }
    }
}