using System;
using System.Collections.Generic;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor;
using CodeBase.Infrastructure.DataProvider;
using UnityEngine;

namespace CodeBase.Services.FurnitureConstructor
{
    public class FurniturePresenter : MonoBehaviour, IFurniturePresenter
    {
        public IEnumerable<Furniture> Furnitures => _furnitures;
        public event Action<Furniture> FurnitureCreated;
        public event Action<Furniture> FurnitureSelected;

        [SerializeField] private FurnitureCatalog _catalog;
        [SerializeField] private Material _glassMaterial;

        private IFurnitureFactory _furnitureFactory;
        private Furniture _currentFurniture;
        private readonly List<Furniture> _furnitures = new List<Furniture>();

        public void Initialize()
        {
            IFurnitureLoader loader = new FurnitureLoader();
            loader.LoadDatabase();
            _furnitureFactory = new FurnitureFactory(loader, _glassMaterial);
        }

        public void LoadCatalog()
        {
            foreach (var prefab in _catalog.Prefabs)
                GenerateFurniture(prefab);
        }

        public void SelectFurniture(Furniture furniture)
        {
            _currentFurniture = furniture;
            UpdateVisibility();
            FurnitureSelected?.Invoke(furniture);
        }

        public void ApplyMaterial(string label, string textureName) =>
            _currentFurniture?.ApplyNewMaterial(label, textureName);

        public void ApplyStyle(string key, string label) =>
            _currentFurniture?.ApplyNewStyle(key, label);

        public void ApplySize(MorphType type, float value) =>
            _currentFurniture?.ApplyNewSize(type, value);

        private void GenerateFurniture(GameObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.name = prefab.name;
            var furniture = _furnitureFactory.CreateFurniture(instance);
            if (furniture == null)
            {
                Debug.LogError($"Ошибка создания мебели: {prefab.name}");
                return;
            }

            _currentFurniture = furniture;
            _furnitures.Add(furniture);
            UpdateVisibility();
            FurnitureCreated?.Invoke(furniture);
        }

        private void UpdateVisibility()
        {
            foreach (var furniture in _furnitures)
                furniture.Prefab.SetActive(furniture == _currentFurniture);
        }
    }
}
