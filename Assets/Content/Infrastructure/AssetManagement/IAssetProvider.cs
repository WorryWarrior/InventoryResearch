﻿using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Content.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IInitializable
    {
        public UniTask<T> Load<T>(string key) where T : class;
        public void Release(string key);
        public void Cleanup();
    }
}