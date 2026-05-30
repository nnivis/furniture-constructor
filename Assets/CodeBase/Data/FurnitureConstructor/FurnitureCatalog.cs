using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data.FurnitureConstructor
{
    [CreateAssetMenu(fileName = "FurnitureCatalog", menuName = "FurnitureConstructor/Catalog")]
    public class FurnitureCatalog : ScriptableObject
    {
        [SerializeField] private List<GameObject> _prefabs;

        public IReadOnlyList<GameObject> Prefabs => _prefabs;
    }
}
