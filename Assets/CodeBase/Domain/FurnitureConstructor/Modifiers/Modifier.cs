using CodeBase.Data.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Domain.FurnitureConstructor.Modifiers
{
    public class Modifier
    {
        private readonly StyleModifier _styleModifier;
        private readonly MaterialModifier _materialModifier;
        private readonly SizeModifier _sizeModifier;

        public Modifier(Material glassMaterial)
        {
            _styleModifier = new StyleModifier();
            _sizeModifier = new SizeModifier();
            _materialModifier = new MaterialModifier(glassMaterial);
        }

        public void SetStartModifier(FurnitureData data, GameObject prefab)
        {
            _styleModifier.InitializeStyle(data, prefab);
            _sizeModifier.InitializeSize(data, prefab);
            _materialModifier.InitializeMaterial(data, prefab);
        }

        public void SetMaterialByLabel(FurnitureData data, GameObject prefab, string label, string textureName) =>
            _materialModifier.SetMaterialByLabel(data, prefab, label, textureName);

        public void SetStyleByKeyAndLabel(FurnitureData data, GameObject prefab, string styleKey, string styleLabel) =>
            _styleModifier.SetStyleByKeyAndLabel(data, prefab, styleKey, styleLabel);

        public void SetSize(FurnitureData data, GameObject prefab, MorphType type, float newValue) =>
            _sizeModifier.SetSize(data, prefab, type, newValue);
    }
}