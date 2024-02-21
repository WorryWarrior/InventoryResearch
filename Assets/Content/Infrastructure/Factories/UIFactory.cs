using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId          = "PFB_UIRoot";
        private const string InventoryHudPrefabId    = "PFB_InventoryHud";

       //private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;

        private Canvas _uiRoot;

        public UIFactory(
            //DiContainer container,
            IAssetProvider assetProvider)
        {
            //_container = container;
            _assetProvider = assetProvider;
        }

        public async UniTask WarmUp()
        {
            await _assetProvider.Load<GameObject>(UIRootPrefabId);
            //await _assetProvider.Load<GameObject>(InventoryHudPrefabId);
        }

        public void CleanUp()
        {
            //_assetProvider.Release(InventoryHudPrefabId);
        }

        public async UniTask CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }
    }
}