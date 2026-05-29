using CodeBase.Domain.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Services.FurnitureConstructor
{
    public interface IFurnitureFactory
    {
        Furniture CreateFurniture(GameObject prefab);
    }
}
