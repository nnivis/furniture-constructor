using System;
using System.Collections.Generic;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Services.FurnitureConstructor
{
    public interface IFurniturePresenter
    {
        IEnumerable<Furniture> Furnitures { get; }
        event Action<Furniture> FurnitureCreated;
        event Action<Furniture> FurnitureSelected;
        void Initialize();
        void LoadCatalog();
        void SelectFurniture(Furniture furniture);
        void ApplyMaterial(string label, string textureName);
        void ApplyStyle(string key, string label);
        void ApplySize(MorphType type, float value);
    }
}
