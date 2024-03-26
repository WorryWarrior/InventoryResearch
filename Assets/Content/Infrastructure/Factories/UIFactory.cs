using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Menu;
using Content.Menu.Equipment;
using Content.Menu.Inventory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId                = "PFB_UIRoot";
        private const string InventoryHudPrefabId          = "PFB_InventoryHUD";
        private const string InventorySlotPrefabId         = "PFB_InventorySlot";
        private const string InventorDragPreviewPrefabId   = "PFB_InventoryDragPreview";
        private const string EquipmentHudPrefabId          = "PFB_EquipmentHUD";

        private readonly IObjectResolver _container;
        private readonly IAssetProvider _assetProvider;

        private Canvas _uiRoot;

        public UIFactory(
            LifetimeScope lifetimeScope,
            IAssetProvider assetProvider)
        {
            _container = lifetimeScope.Container;
            _assetProvider = assetProvider;
        }

        public async UniTask WarmUp()
        {
            await _assetProvider.Load<GameObject>(UIRootPrefabId);
            await _assetProvider.Load<GameObject>(InventoryHudPrefabId);
            await _assetProvider.Load<GameObject>(InventorySlotPrefabId);
            await _assetProvider.Load<GameObject>(InventorDragPreviewPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(InventoryHudPrefabId);
            _assetProvider.Release(InventorySlotPrefabId);
            _assetProvider.Release(InventorDragPreviewPrefabId);
        }

        public async UniTask CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async UniTask<InventoryHUDController> CreateInventoryHUD()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventoryHudPrefabId);
            InventoryHUDController hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<InventoryHUDController>();

            _container.InjectGameObject(hud.gameObject);
            return hud;
        }

        public async UniTask<InventorySlotController> CreateInventorySlot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventorySlotPrefabId);
            InventorySlotController slot = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<InventorySlotController>();

            //_container.InjectGameObject(slot.gameObject);
            return slot;
        }

        public async UniTask<InventoryDragPreviewController> CreateInventoryDragPreview()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventorDragPreviewPrefabId);
            InventoryDragPreviewController dragPreview = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<InventoryDragPreviewController>();

            dragPreview.Initialize(_uiRoot);
            //_container.InjectGameObject(dragPreview.gameObject);
            return dragPreview;
        }

        public async UniTask<EquipmentHUDController> CreateEquipmentHUD()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(EquipmentHudPrefabId);
            EquipmentHUDController hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<EquipmentHUDController>();

            _container.InjectGameObject(hud.gameObject);
            return hud;
        }
    }
}