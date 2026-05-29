using System.Collections.Generic;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor.Modifiers;
using CodeBase.Infrastructure.DataProvider;
using UnityEngine;

namespace CodeBase.Services.FurnitureConstructor
{
    public class FurnitureFactory : IFurnitureFactory
    {
        private readonly IFurnitureLoader _furnitureLoader;
        private readonly Material _glassMaterial;

        public FurnitureFactory(IFurnitureLoader furnitureLoader, Material glassMaterial)
        {
            _furnitureLoader = furnitureLoader;
            _glassMaterial = glassMaterial;
        }

        public Furniture CreateFurniture(GameObject prefab)
        {
            if (prefab == null) return null;

            var data = _furnitureLoader.GetFurnitureData(prefab.name);
            if (data == null)
            {
                Debug.LogError($"Failed to get FurnitureData for: {prefab.name}");
                return null;
            }

            SaveMorphUVs(prefab.transform, data);
            RemoveMorphElements(prefab.transform);

            var modifier = new Modifier(_glassMaterial);
            var furnitureComponent = prefab.AddComponent<Furniture>();
            furnitureComponent.Initialize(prefab, data, modifier);

            return furnitureComponent;
        }

        private void SaveMorphUVs(Transform parent, FurnitureData data)
        {
            foreach (Transform child in parent)
            {
                if (!child.name.ToLower().Contains(FurnitureConstants.Morph)) continue;

                if (child.childCount > 0)
                {
                    foreach (Transform subChild in child)
                    {
                        SaveUV(subChild, data);
                    }
                }
                else
                {
                    SaveUV(child, data);
                }
            }
        }

        private void SaveUV(Transform obj, FurnitureData data)
        {
            var meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter?.sharedMesh == null) return;

            string name = obj.name;
            MorphType? morphType = GetMorphType(name);
            if (morphType.HasValue)
            {
                data.AddUV(morphType.Value, name, meshFilter.sharedMesh.uv);
            }
        }

        private MorphType? GetMorphType(string name)
        {
            var lowerName = name.ToLower();
            if (lowerName.Contains(FurnitureConstants.Height)) return MorphType.Height;
            if (lowerName.Contains(FurnitureConstants.Width)) return MorphType.Width;
            if (lowerName.Contains(FurnitureConstants.Depth)) return MorphType.Depth;
            return null;
        }

        private void RemoveMorphElements(Transform parent)
        {
            var morphElements = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (child.name.ToLower().Contains(FurnitureConstants.Morph))
                {
                    morphElements.Add(child);
                }
            }

            foreach (Transform morphElement in morphElements)
            {
                Object.Destroy(morphElement.gameObject);
            }
        }
    }
}
