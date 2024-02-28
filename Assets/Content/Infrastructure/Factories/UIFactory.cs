using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Menu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId          = "PFB_UIRoot";
        private const string InventoryHudPrefabId    = "PFB_InventoryHUD";
        private const string InventorySlotPrefabId   = "PFB_InventorySlot";

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
        }

        public void CleanUp()
        {
            _assetProvider.Release(InventoryHudPrefabId);
            _assetProvider.Release(InventorySlotPrefabId);
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

            _container.InjectGameObject(slot.gameObject);
            return slot;
        }
    }
}