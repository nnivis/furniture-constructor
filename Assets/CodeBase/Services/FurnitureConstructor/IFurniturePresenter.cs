using System;
using System.Collections.Generic;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor;

namespace CodeBase.Services.FurnitureConstructor
{
    public interface IFurniturePresenter
    {
        IEnumerable<Furniture> Furnitures { get; }
        event Action<Furniture> FurnitureCreated;
        event Action<Furniture> FurnitureSelected;
        void Initialize(IFurnitureFactory factory);
        void LoadCatalog();
        void SelectFurniture(Furniture furniture);
        void ApplyMaterial(string label, string textureName);
        void ApplyStyle(string key, string label);
        void ApplySize(MorphType type, float value);
    }
}
