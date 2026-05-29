using System.Collections.Generic;
using System.Linq;
using CodeBase.Data.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Domain.FurnitureConstructor.Modifiers
{
    public class StyleModifier
    {
        private readonly Dictionary<string, string> _activeStyles = new Dictionary<string, string>();
        private readonly HashSet<string> _allNameInModels = new HashSet<string>();

        public void InitializeStyle(FurnitureData data, GameObject prefab)
        {
            if (data?.Parts == null || prefab == null)
                return;

            _activeStyles.Clear();
            _allNameInModels.Clear();
            var activeNameInModels = new HashSet<string>();

            foreach (var part in data.Parts)
            {
                foreach (var styleEntry in part.Value.styles)
                {
                    foreach (var style in styleEntry.Value)
                    {
                        _allNameInModels.Add(style.nameInModel);
                    }

                    if (styleEntry.Value.Count > 0)
                    {
                        var startStyle = styleEntry.Value[0];
                        activeNameInModels.Add(startStyle.nameInModel);
                        _activeStyles[styleEntry.Key] = startStyle.label;
                    }
                }
            }

            ApplyStyleToChildren(prefab, activeNameInModels);
        }

        public void SetStyleByKeyAndLabel(FurnitureData data, GameObject prefab, string styleKey, string styleLabel)
        {
            if (data?.Parts == null || prefab == null)
                return;

            _activeStyles[styleKey] = styleLabel;

            var activeNameInModels = new HashSet<string>();

            foreach (var part in data.Parts)
            {
                foreach (var styleEntry in part.Value.styles)
                {
                    foreach (var styleInfo in styleEntry.Value)
                    {
                        if (_activeStyles.TryGetValue(styleEntry.Key, out var activeLabel) &&
                            styleInfo.label == activeLabel)
                        {
                            activeNameInModels.Add(styleInfo.nameInModel);
                        }
                    }
                }
            }

            ApplyStyleToChildren(prefab, activeNameInModels);
        }

        private void ApplyStyleToChildren(GameObject parent, HashSet<string> activeNameInModels)
        {
            foreach (Transform child in parent.transform)
            {
                string childName = child.name;

                if (_allNameInModels.Any(n => childName.Contains(n)))
                {
                    bool isActive = activeNameInModels.Any(n => childName.Contains(n));
                    child.gameObject.SetActive(isActive);
                }

                if (child.childCount > 0)
                    ApplyStyleToChildren(child.gameObject, activeNameInModels);
            }
        }
    }
}
